using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Worldline.Acquiring.Sdk.Logging;
using Worldline.Acquiring.Sdk.Util;

namespace Worldline.Acquiring.Sdk.Communication
{
    /// <summary>
    /// Verifies how <see cref="DefaultConnection"/> handles a <see cref="CancellationToken"/>.
    /// The requests are answered by a handler in the test itself, so no network is involved.
    /// </summary>
    [TestFixture]
    public class DefaultConnectionCancellationTest
    {
        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(10);
        private static readonly Uri ResourceUri = new Uri("https://example.com/v1/resource");
        private static readonly List<IRequestHeader> NoHeaders = new List<IRequestHeader>();

        private static HttpResponseMessage Ok(string body)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
        }

        private static DefaultConnection CreateConnection(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> send, TimeSpan? timeout = null)
        {
            // this constructor disposes the HttpClient after each request, so a new one is created for every request
            return new DefaultConnection(() =>
            {
                var client = new HttpClient(new DelegateHandler(send));
                if (timeout != null)
                {
                    client.Timeout = timeout.Value;
                }
                return client;
            });
        }

        private static async Task<string> ReadBody(HttpStatusCode status, Stream body, IEnumerable<IResponseHeader> headers)
        {
            using (var reader = new StreamReader(body))
            {
                return status + ":" + await reader.ReadToEndAsync().ConfigureAwait(false);
            }
        }

        // cancels the given token as soon as a response is logged
        private sealed class CancellingLogger : ICommunicatorLogger
        {
            private readonly CancellationTokenSource _cts;

            public CancellingLogger(CancellationTokenSource cts)
            {
                _cts = cts;
            }

            public void Log(string message)
            {
                if (message.StartsWith("Incoming response", StringComparison.Ordinal))
                {
                    _cts.Cancel();
                }
            }

            public void Log(string message, Exception thrown)
            {
            }
        }

        private static async Task WaitFor(Task task)
        {
            var finished = await Task.WhenAny(task, Task.Delay(TestTimeout)).ConfigureAwait(false);
            Assert.That(finished, Is.SameAs(task), "timed out waiting for the test condition");
        }

        // A test that waits for a task that never completes would hang, so a task that takes too long fails the test instead
        private static async Task<T> WithTimeout<T>(Task<T> task)
        {
            var finished = await Task.WhenAny(task, Task.Delay(TestTimeout)).ConfigureAwait(false);
            if (finished != task)
            {
                throw new TimeoutException("the call did not complete after it was cancelled");
            }
            return await task.ConfigureAwait(false);
        }

        [Test]
        public void TestCancelBeforeSendSendsNothing()
        {
            var requests = 0;
            var connection = CreateConnection((request, token) =>
            {
                Interlocked.Increment(ref requests);
                return Task.FromResult(Ok("{}"));
            });

            using (var cts = new CancellationTokenSource())
            {
                cts.Cancel();

                var exception = Assert.CatchAsync<OperationCanceledException>(async () => await connection.Get(ResourceUri, NoHeaders, ReadBody, cts.Token));

                Assert.That(exception, Is.Not.InstanceOf<CommunicationException>());
            }
            Assert.That(requests, Is.EqualTo(0));
        }

        [Test]
        public void TestEveryMethodStopsForACancelledToken()
        {
            var requests = 0;
            var connection = CreateConnection((request, token) =>
            {
                Interlocked.Increment(ref requests);
                return Task.FromResult(Ok("{}"));
            });
            var multipart = new MultipartFormDataObject();
            multipart.AddValue("name", "value");

            using (var cts = new CancellationTokenSource())
            {
                cts.Cancel();
                var calls = new Dictionary<string, Func<Task<string>>>
                {
                    ["GET"] = () => connection.Get(ResourceUri, NoHeaders, ReadBody, cts.Token),
                    ["DELETE"] = () => connection.Delete(ResourceUri, NoHeaders, ReadBody, cts.Token),
                    ["POST"] = () => connection.Post(ResourceUri, NoHeaders, "body", ReadBody, cts.Token),
                    ["POST multipart"] = () => connection.Post(ResourceUri, NoHeaders, multipart, ReadBody, cts.Token),
                    ["PUT"] = () => connection.Put(ResourceUri, NoHeaders, "body", ReadBody, cts.Token),
                    ["PUT multipart"] = () => connection.Put(ResourceUri, NoHeaders, multipart, ReadBody, cts.Token)
                };

                foreach (var call in calls)
                {
                    Assert.CatchAsync<OperationCanceledException>(async () => await call.Value(), call.Key);
                }
            }
            Assert.That(requests, Is.EqualTo(0));
        }

        [Test]
        public async Task TestCancelDuringSendStopsTheRequest()
        {
            var started = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var connection = CreateConnection(async (request, token) =>
            {
                started.TrySetResult(true);
                await Task.Delay(Timeout.Infinite, token).ConfigureAwait(false);
                return Ok("{}");
            });

            using (var cts = new CancellationTokenSource())
            {
                var call = connection.Get(ResourceUri, NoHeaders, ReadBody, cts.Token);
                await WaitFor(started.Task);
                Assert.That(call.IsCompleted, Is.False, "the request must still be in flight");

                cts.Cancel();

                var exception = Assert.CatchAsync<OperationCanceledException>(async () => await WithTimeout(call));
                // a cancellation is not a communication problem
                Assert.That(exception, Is.Not.InstanceOf<CommunicationException>());
            }
        }

        [Test]
        public async Task TestCancelDuringAnAsynchronousResponseHandlerStopsTheCall()
        {
            var started = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var connection = CreateConnection((request, token) => Task.FromResult(Ok("{}")));

            using (var cts = new CancellationTokenSource())
            {
                var call = connection.Get<string>(ResourceUri, NoHeaders, async (status, body, headers) =>
                {
                    started.TrySetResult(true);
                    await Task.Delay(Timeout.Infinite, cts.Token).ConfigureAwait(false);
                    return "never";
                }, cts.Token);
                await WaitFor(started.Task);
                Assert.That(call.IsCompleted, Is.False, "the handler must still be running");

                cts.Cancel();

                var exception = Assert.CatchAsync<OperationCanceledException>(async () => await WithTimeout(call));
                Assert.That(exception, Is.Not.InstanceOf<CommunicationException>());
            }
        }

        [Test]
        public async Task TestCancelAfterTheResponseHasArrivedDoesNotDiscardIt()
        {
            var connection = CreateConnection((request, token) => Task.FromResult(Ok("{\"name\":\"value\"}")));
            var handlerCalled = false;

            using (var cts = new CancellationTokenSource())
            {
                // the logger is called after the response has been received and read, just before the response handler, so it cancels at exactly that moment
                connection.EnableLogging(new CancellingLogger(cts));

                var result = await WithTimeout(connection.Get<string>(ResourceUri, NoHeaders, async (status, body, headers) =>
                {
                    handlerCalled = true;
                    return await ReadBody(status, body, headers).ConfigureAwait(false);
                }, cts.Token));

                Assert.That(cts.IsCancellationRequested, Is.True, "the token must have been cancelled while the response was being logged");
                Assert.That(handlerCalled, Is.True);
                Assert.That(result, Is.EqualTo("OK:{\"name\":\"value\"}"));
            }
        }

        [Test]
        public void TestTimeoutIsStillACommunicationException()
        {
            // HttpClient reports a timeout as a TaskCanceledException too. That is not a cancellation by the caller, so it must stay a CommunicationException.
            var connection = CreateConnection(async (request, token) =>
            {
                await Task.Delay(Timeout.Infinite, token).ConfigureAwait(false);
                return Ok("{}");
            }, TimeSpan.FromMilliseconds(100));

            // without a token, which is the existing method
            var exception = Assert.CatchAsync<CommunicationException>(async () => await ((IConnection)connection).Get(ResourceUri, NoHeaders, (status, body, headers) => "never"));
            Assert.That(exception.InnerException, Is.InstanceOf<TaskCanceledException>());

            // and with a token that is not cancelled
            using (var cts = new CancellationTokenSource())
            {
                var exceptionWithToken = Assert.CatchAsync<CommunicationException>(async () => await connection.Get(ResourceUri, NoHeaders, ReadBody, cts.Token));
                Assert.That(exceptionWithToken.InnerException, Is.InstanceOf<TaskCanceledException>());
            }
        }

        [Test]
        public async Task TestTheResponseHandlerIsAwaitedWhileTheResponseIsOpen()
        {
            var connection = CreateConnection((request, token) => Task.FromResult(Ok("{\"name\":\"value\"}")));

            var result = await connection.Get<string>(ResourceUri, NoHeaders, async (status, body, headers) =>
            {
                // really asynchronous: the stream is only read after the handler has given up the thread
                await Task.Yield();
                using (var reader = new StreamReader(body))
                {
                    return status + ":" + await reader.ReadToEndAsync().ConfigureAwait(false);
                }
            }, CancellationToken.None);

            Assert.That(result, Is.EqualTo("OK:{\"name\":\"value\"}"));
        }

        [Test]
        public void TestAnExceptionFromTheResponseHandlerIsNotChanged()
        {
            var connection = CreateConnection((request, token) => Task.FromResult(Ok("{}")));

            var exception = Assert.CatchAsync<InvalidOperationException>(async () =>
                await connection.Get<string>(ResourceUri, NoHeaders, (status, body, headers) => throw new InvalidOperationException("handler failed"), CancellationToken.None));

            Assert.That(exception.Message, Is.EqualTo("handler failed"));
        }

        [Test]
        public async Task TestExistingMethodsStillWorkWithoutAToken()
        {
            string receivedBody = null;
            string receivedMethod = null;
            var connection = CreateConnection(async (request, token) =>
            {
                receivedMethod = request.Method.Method;
                receivedBody = request.Content == null ? null : await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                return Ok("{\"result\":true}");
            });
            IConnection plain = connection;

            Assert.That(await plain.Get(ResourceUri, NoHeaders, (status, body, headers) => status), Is.EqualTo(HttpStatusCode.OK));
            Assert.That(receivedMethod, Is.EqualTo("GET"));

            Assert.That(await plain.Delete(ResourceUri, NoHeaders, (status, body, headers) => status), Is.EqualTo(HttpStatusCode.OK));
            Assert.That(receivedMethod, Is.EqualTo("DELETE"));

            Assert.That(await plain.Post(ResourceUri, NoHeaders, "post body", (status, body, headers) => status), Is.EqualTo(HttpStatusCode.OK));
            Assert.That(receivedMethod, Is.EqualTo("POST"));
            Assert.That(receivedBody, Is.EqualTo("post body"));

            Assert.That(await plain.Put(ResourceUri, NoHeaders, "put body", (status, body, headers) => status), Is.EqualTo(HttpStatusCode.OK));
            Assert.That(receivedMethod, Is.EqualTo("PUT"));
            Assert.That(receivedBody, Is.EqualTo("put body"));

            var multipart = new MultipartFormDataObject();
            multipart.AddValue("name", "value");
            Assert.That(await plain.Post(ResourceUri, NoHeaders, multipart, (status, body, headers) => status), Is.EqualTo(HttpStatusCode.OK));
            Assert.That(receivedMethod, Is.EqualTo("POST"));
            Assert.That(receivedBody, Does.Contain("name=name"));

            Assert.That(await plain.Put(ResourceUri, NoHeaders, multipart, (status, body, headers) => status), Is.EqualTo(HttpStatusCode.OK));
            Assert.That(receivedMethod, Is.EqualTo("PUT"));
        }

        [Test]
        public async Task TestTheTokenDoesNotChangeTheResult()
        {
            var connection = CreateConnection((request, token) => Task.FromResult(Ok("{\"name\":\"value\"}")));

            using (var cts = new CancellationTokenSource())
            {
                Assert.That(await connection.Get(ResourceUri, NoHeaders, ReadBody, cts.Token), Is.EqualTo("OK:{\"name\":\"value\"}"));
                Assert.That(await connection.Get(ResourceUri, NoHeaders, ReadBody, CancellationToken.None), Is.EqualTo("OK:{\"name\":\"value\"}"));
            }
        }
    }
}
