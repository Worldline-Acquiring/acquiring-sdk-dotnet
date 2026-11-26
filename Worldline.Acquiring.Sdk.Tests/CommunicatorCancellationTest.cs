using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Worldline.Acquiring.Sdk.Authentication;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.Json;
using Worldline.Acquiring.Sdk.Logging;

namespace Worldline.Acquiring.Sdk
{
    /// <summary>
    /// Verifies how <see cref="Communicator"/> passes a <see cref="CancellationToken"/> on, and that it still works with a connection or an
    /// authenticator that only implements the existing interfaces.
    /// </summary>
    [TestFixture]
    public class CommunicatorCancellationTest
    {
        private const string Path = "/v1/resource";

        public class TestResponse
        {
            public string Name { get; set; }
        }

        private static Communicator CreateCommunicator(IConnection connection, IAuthenticator authenticator)
        {
            return new Communicator(new Uri("https://api.example.com"), connection, authenticator, new MetadataProvider("Worldline"), DefaultMarshaller.Instance);
        }

        // the two kinds of connection and authenticator that a communicator has to work with

        private static IEnumerable<TestCaseData> ConnectionKinds()
        {
            yield return new TestCaseData(false).SetName("plain connection and authenticator");
            yield return new TestCaseData(true).SetName("cancellable connection and authenticator");
        }

        private static (FakeConnection Connection, FakeAuthenticator Authenticator, Communicator Communicator) Create(bool cancellable)
        {
            if (cancellable)
            {
                var cancellableConnection = new CancellableConnection();
                var cancellableAuthenticator = new CancellableAuthenticator();
                return (cancellableConnection, cancellableAuthenticator, CreateCommunicator(cancellableConnection, cancellableAuthenticator));
            }

            var plainConnection = new PlainConnection();
            var plainAuthenticator = new PlainAuthenticator();
            return (plainConnection, plainAuthenticator, CreateCommunicator(plainConnection, plainAuthenticator));
        }

        [TestCaseSource(nameof(ConnectionKinds))]
        public async Task TestGetReturnsTheUnmarshalledResponse(bool cancellable)
        {
            var (connection, authenticator, communicator) = Create(cancellable);

            var response = await communicator.Get<TestResponse>(Path, null, null, null, CancellationToken.None);

            Assert.That(response.Name, Is.EqualTo("value"));
            Assert.That(connection.Calls, Is.EqualTo(new[] { "GET" }));
            Assert.That(authenticator.Calls, Is.EqualTo(1));
        }

        [Test]
        public async Task TestACancellableConnectionAndAuthenticatorReceiveTheToken()
        {
            var (connection, authenticator, communicator) = Create(true);

            using (var cts = new CancellationTokenSource())
            {
                await communicator.Get<TestResponse>(Path, null, null, null, cts.Token);

                Assert.That(connection.LastToken, Is.EqualTo(cts.Token));
                Assert.That(authenticator.LastToken, Is.EqualTo(cts.Token));
            }
        }

        [Test]
        public async Task TestEveryMethodPassesTheTokenAndUsesTheRightHttpMethod()
        {
            var (connection, authenticator, communicator) = Create(true);
            var multipart = new MultipartFormDataObject();
            multipart.AddValue("name", "value");

            using (var cts = new CancellationTokenSource())
            {
                var token = cts.Token;
                await communicator.Get<TestResponse>(Path, null, null, null, token);
                await communicator.Delete<TestResponse>(Path, null, null, null, token);
                await communicator.Post<TestResponse>(Path, null, null, new TestResponse { Name = "request" }, null, token);
                await communicator.Put<TestResponse>(Path, null, null, new TestResponse { Name = "request" }, null, token);
                await communicator.Post<TestResponse>(Path, null, null, multipart, null, token);
                await communicator.Put<TestResponse>(Path, null, null, multipart, null, token);

                Assert.That(connection.Calls, Is.EqualTo(new[] { "GET", "DELETE", "POST", "PUT", "POST multipart", "PUT multipart" }));
                Assert.That(connection.Tokens, Is.All.EqualTo(token));
                Assert.That(authenticator.Tokens, Is.All.EqualTo(token));
                Assert.That(connection.Tokens.Count, Is.EqualTo(6));
            }
        }

        [Test]
        public async Task TestEveryBodyHandlerMethodPassesTheTokenAndUsesTheRightHttpMethod()
        {
            var (connection, authenticator, communicator) = Create(true);
            var multipart = new MultipartFormDataObject();
            multipart.AddValue("name", "value");
            Func<Stream, IEnumerable<IResponseHeader>, Task> handler = (stream, headers) => Task.CompletedTask;

            using (var cts = new CancellationTokenSource())
            {
                var token = cts.Token;
                await communicator.Get(Path, null, null, handler, null, token);
                await communicator.Delete(Path, null, null, handler, null, token);
                await communicator.Post(Path, null, null, new TestResponse { Name = "request" }, handler, null, token);
                await communicator.Put(Path, null, null, new TestResponse { Name = "request" }, handler, null, token);
                await communicator.Post(Path, null, null, multipart, handler, null, token);
                await communicator.Put(Path, null, null, multipart, handler, null, token);

                Assert.That(connection.Calls, Is.EqualTo(new[] { "GET", "DELETE", "POST", "PUT", "POST multipart", "PUT multipart" }));
                Assert.That(connection.Tokens, Is.All.EqualTo(token));
                Assert.That(authenticator.Tokens, Is.All.EqualTo(token));
            }
        }

        [TestCaseSource(nameof(ConnectionKinds))]
        public async Task TestTheExistingMethodsNeedNoTokenAndPassNone(bool cancellable)
        {
            var (connection, authenticator, communicator) = Create(cancellable);

            var response = await communicator.Get<TestResponse>(Path, null, null, null);
            await communicator.Post<TestResponse>(Path, null, null, new TestResponse { Name = "request" }, null);

            Assert.That(response.Name, Is.EqualTo("value"));
            Assert.That(connection.Calls, Is.EqualTo(new[] { "GET", "POST" }));
            if (cancellable)
            {
                Assert.That(connection.Tokens, Is.All.EqualTo(CancellationToken.None));
                Assert.That(authenticator.Tokens, Is.All.EqualTo(CancellationToken.None));
            }
        }

        [TestCaseSource(nameof(ConnectionKinds))]
        public void TestACancelledTokenStopsBeforeAnythingIsSent(bool cancellable)
        {
            var (connection, authenticator, communicator) = Create(cancellable);

            using (var cts = new CancellationTokenSource())
            {
                cts.Cancel();

                Assert.CatchAsync<OperationCanceledException>(async () => await communicator.Get<TestResponse>(Path, null, null, null, cts.Token));
                Assert.CatchAsync<OperationCanceledException>(async () => await communicator.Post<TestResponse>(Path, null, null, new TestResponse(), null, cts.Token));
            }

            Assert.That(connection.Calls, Is.Empty, "nothing may be sent");
            if (!cancellable)
            {
                // a plain authenticator cannot be cancelled, so it must not even be asked
                Assert.That(authenticator.Calls, Is.EqualTo(0));
            }
        }

        [Test]
        public void TestAPlainAuthenticatorWithACancellableConnectionIsNotCalledForACancelledToken()
        {
            var connection = new CancellableConnection();
            var authenticator = new PlainAuthenticator();
            var communicator = CreateCommunicator(connection, authenticator);

            using (var cts = new CancellationTokenSource())
            {
                cts.Cancel();

                Assert.CatchAsync<OperationCanceledException>(async () => await communicator.Get<TestResponse>(Path, null, null, null, cts.Token));
            }

            Assert.That(authenticator.Calls, Is.EqualTo(0));
            Assert.That(connection.Calls, Is.Empty);
        }

        [TestCaseSource(nameof(ConnectionKinds))]
        public async Task TestTheGenericHeadersAreAdded(bool cancellable)
        {
            var (connection, authenticator, communicator) = Create(cancellable);

            await communicator.Get<TestResponse>(Path, null, null, null, CancellationToken.None);

            var headers = connection.LastRequestHeaders.ToDictionary(header => header.Name, header => header.Value);
            Assert.That(headers.Keys, Does.Contain("Authorization"));
            Assert.That(headers["Authorization"], Is.EqualTo(authenticator.Authorization));
            Assert.That(headers.Keys, Does.Contain("Date"));
            Assert.That(headers.Keys, Does.Contain("Trace-Id"));
            Assert.That(Guid.TryParse(headers["Trace-Id"], out _), Is.True, "the trace id is a GUID");

            // and every request has a trace id of its own
            await communicator.Get<TestResponse>(Path, null, null, null, CancellationToken.None);
            var secondTraceId = connection.LastRequestHeaders.First(header => header.Name == "Trace-Id").Value;
            Assert.That(secondTraceId, Is.Not.EqualTo(headers["Trace-Id"]));
        }

        [TestCaseSource(nameof(ConnectionKinds))]
        public void TestAnErrorResponseIsAResponseException(bool cancellable)
        {
            var (connection, authenticator, communicator) = Create(cancellable);
            connection.Status = HttpStatusCode.BadRequest;
            connection.Body = "{\"errorId\":\"1\"}";

            var exception = Assert.CatchAsync<ResponseException>(async () => await communicator.Get<TestResponse>(Path, null, null, null, CancellationToken.None));

            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(exception.Body, Is.EqualTo("{\"errorId\":\"1\"}"));
        }

        [TestCaseSource(nameof(ConnectionKinds))]
        public void TestAMissingResourceWithoutAJsonBodyIsANotFoundException(bool cancellable)
        {
            var (connection, authenticator, communicator) = Create(cancellable);
            connection.Status = HttpStatusCode.NotFound;
            connection.Body = "";

            Assert.CatchAsync<NotFoundException>(async () => await communicator.Get<TestResponse>(Path, null, null, null, CancellationToken.None));
        }

        // binary responses

        private const string PdfBody = "%PDF binary content";

        [TestCaseSource(nameof(ConnectionKinds))]
        public async Task TestAnAsynchronousBodyHandlerReceivesTheBodyAndTheHeaders(bool cancellable)
        {
            var (connection, authenticator, communicator) = Create(cancellable);
            connection.Body = PdfBody;
            connection.ContentType = "application/pdf";
            string received = null;
            string receivedContentType = null;

            await communicator.Get(Path, null, null, async (stream, headers) =>
            {
                // really asynchronous: the stream is only read after the handler has given up the thread
                await Task.Yield();
                using (var reader = new StreamReader(stream))
                {
                    received = await reader.ReadToEndAsync();
                }
                receivedContentType = headers.First(header => header.Name == "Content-Type").Value;
            }, null, CancellationToken.None);

            Assert.That(received, Is.EqualTo(PdfBody));
            Assert.That(receivedContentType, Is.EqualTo("application/pdf"));
        }

        [TestCaseSource(nameof(ConnectionKinds))]
        public async Task TestTheExistingBodyHandlerStillWorks(bool cancellable)
        {
            var (connection, authenticator, communicator) = Create(cancellable);
            connection.Body = PdfBody;
            connection.ContentType = "application/pdf";
            string received = null;

            await communicator.Get(Path, null, null, (Stream stream, IEnumerable<IResponseHeader> headers) =>
            {
                using (var reader = new StreamReader(stream))
                {
                    received = reader.ReadToEnd();
                }
            }, null);

            Assert.That(received, Is.EqualTo(PdfBody));
        }

        [TestCaseSource(nameof(ConnectionKinds))]
        public void TestAnExceptionFromTheBodyHandlerIsWrapped(bool cancellable)
        {
            var (connection, authenticator, communicator) = Create(cancellable);
            connection.Body = PdfBody;
            connection.ContentType = "application/pdf";

            var exception = Assert.CatchAsync<BodyHandlerException>(async () => await communicator.Get(Path, null, null, (stream, headers) =>
            {
                throw new InvalidOperationException("handler failed");
            }, null, CancellationToken.None));

            Assert.That(exception.InnerException, Is.InstanceOf<InvalidOperationException>());
            Assert.That(exception.InnerException.Message, Is.EqualTo("handler failed"));
        }

        [TestCaseSource(nameof(ConnectionKinds))]
        public void TestAnErrorResponseDoesNotCallTheBodyHandler(bool cancellable)
        {
            var (connection, authenticator, communicator) = Create(cancellable);
            connection.Status = HttpStatusCode.BadRequest;
            connection.Body = "{\"errorId\":\"1\"}";
            var called = false;

            Assert.CatchAsync<ResponseException>(async () => await communicator.Get(Path, null, null, (stream, headers) =>
            {
                called = true;
                return Task.CompletedTask;
            }, null, CancellationToken.None));

            Assert.That(called, Is.False);
        }

        [Test]
        public void TestCancellingInTheBodyHandlerIsNotWrappedInABodyHandlerException()
        {
            var (connection, authenticator, communicator) = Create(true);
            connection.Body = PdfBody;

            using (var cts = new CancellationTokenSource())
            {
                var exception = Assert.CatchAsync<OperationCanceledException>(async () => await communicator.Get(Path, null, null, (stream, headers) =>
                {
                    cts.Cancel();
                    cts.Token.ThrowIfCancellationRequested();
                    return Task.CompletedTask;
                }, null, cts.Token));

                Assert.That(exception, Is.Not.InstanceOf<BodyHandlerException>());
            }
        }

        [Test]
        public void TestCancellingDuringAPlainConnectionsCallIsOnlyHonouredBeforeItIsSent()
        {
            // a connection that does not implement ICancellableConnection cannot be interrupted; the call completes, and the token is not an error afterwards
            var (connection, authenticator, communicator) = Create(false);

            using (var cts = new CancellationTokenSource())
            {
                var response = communicator.Get<TestResponse>(Path, null, null, null, cts.Token).GetAwaiter().GetResult();
                cts.Cancel();

                Assert.That(response.Name, Is.EqualTo("value"));
            }
        }

        [TestCaseSource(nameof(ConnectionKinds))]
        public async Task TestCancellingAfterTheResponseHasArrivedDoesNotDiscardIt(bool cancellable)
        {
            var (connection, authenticator, communicator) = Create(cancellable);

            using (var cts = new CancellationTokenSource())
            {
                connection.OnResponse = cts.Cancel;

                var response = await communicator.Get<TestResponse>(Path, null, null, null, cts.Token);

                Assert.That(cts.IsCancellationRequested, Is.True);
                Assert.That(response.Name, Is.EqualTo("value"));
            }
        }

        [TestCaseSource(nameof(ConnectionKinds))]
        public async Task TestCancellingAfterTheResponseHasArrivedStillCallsTheBodyHandler(bool cancellable)
        {
            var (connection, authenticator, communicator) = Create(cancellable);
            connection.Body = PdfBody;
            connection.ContentType = "application/pdf";
            string received = null;

            using (var cts = new CancellationTokenSource())
            {
                connection.OnResponse = cts.Cancel;

                await communicator.Get(Path, null, null, async (stream, headers) =>
                {
                    using (var reader = new StreamReader(stream))
                    {
                        received = await reader.ReadToEndAsync();
                    }
                }, null, cts.Token);

                Assert.That(cts.IsCancellationRequested, Is.True);
            }

            Assert.That(received, Is.EqualTo(PdfBody));
        }

        // fakes

        private abstract class FakeConnection : ILoggingCapable, IObfuscationCapable, IDisposable
        {
            public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;

            public string Body { get; set; } = "{\"name\":\"value\"}";

            public string ContentType { get; set; } = "application/json";

            public List<string> Calls { get; } = new List<string>();

            public List<CancellationToken> Tokens { get; } = new List<CancellationToken>();

            public CancellationToken? LastToken => Tokens.Count == 0 ? (CancellationToken?)null : Tokens[Tokens.Count - 1];

            // called when the response is available, just before it is handed to the response handler
            public Action OnResponse { get; set; }

            public IEnumerable<IRequestHeader> LastRequestHeaders { get; private set; }

            protected void Record(string method, IEnumerable<IRequestHeader> requestHeaders, bool multipart)
            {
                Calls.Add(multipart ? method + " multipart" : method);
                LastRequestHeaders = requestHeaders.ToList();
            }

            protected Stream ResponseBody()
            {
                return new MemoryStream(Encoding.UTF8.GetBytes(Body));
            }

            protected IEnumerable<IResponseHeader> ResponseHeaders()
            {
                return new List<IResponseHeader> { new ResponseHeader("Content-Type", ContentType) };
            }

            public void EnableLogging(ICommunicatorLogger communicatorLogger)
            {
            }

            public void DisableLogging()
            {
            }

            public BodyObfuscator BodyObfuscator
            {
                set { }
            }

            public HeaderObfuscator HeaderObfuscator
            {
                set { }
            }

            public void Dispose()
            {
            }
        }

        // only implements IConnection, like every connection that existed before cancellation support
        private sealed class PlainConnection : FakeConnection, IConnection
        {
            private Task<T> Respond<T>(string method, IEnumerable<IRequestHeader> requestHeaders, bool multipart, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, T> responseHandler)
            {
                Record(method, requestHeaders, multipart);
                OnResponse?.Invoke();
                return Task.FromResult(responseHandler(Status, ResponseBody(), ResponseHeaders()));
            }

            public Task<T> Get<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, T> responseHandler)
            {
                return Respond("GET", requestHeaders, false, responseHandler);
            }

            public Task<T> Delete<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, T> responseHandler)
            {
                return Respond("DELETE", requestHeaders, false, responseHandler);
            }

            public Task<T> Post<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, string body, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, T> responseHandler)
            {
                return Respond("POST", requestHeaders, false, responseHandler);
            }

            public Task<T> Post<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, MultipartFormDataObject multipart, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, T> responseHandler)
            {
                return Respond("POST", requestHeaders, true, responseHandler);
            }

            public Task<T> Put<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, string body, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, T> responseHandler)
            {
                return Respond("PUT", requestHeaders, false, responseHandler);
            }

            public Task<T> Put<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, MultipartFormDataObject multipart, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, T> responseHandler)
            {
                return Respond("PUT", requestHeaders, true, responseHandler);
            }
        }

        // implements ICancellableConnection; the plain IConnection methods are not expected to be used by the communicator
        private sealed class CancellableConnection : FakeConnection, ICancellableConnection
        {
            private Task<T> Respond<T>(string method, IEnumerable<IRequestHeader> requestHeaders, bool multipart, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, Task<T>> responseHandler, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Record(method, requestHeaders, multipart);
                Tokens.Add(cancellationToken);
                OnResponse?.Invoke();
                return responseHandler(Status, ResponseBody(), ResponseHeaders());
            }

            public Task<T> Get<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, T> responseHandler)
            {
                return Get(uri, requestHeaders, (status, body, headers) => Task.FromResult(responseHandler(status, body, headers)), CancellationToken.None);
            }

            public Task<T> Delete<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, T> responseHandler)
            {
                return Delete(uri, requestHeaders, (status, body, headers) => Task.FromResult(responseHandler(status, body, headers)), CancellationToken.None);
            }

            public Task<T> Post<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, string body, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, T> responseHandler)
            {
                return Post(uri, requestHeaders, body, (status, responseBody, headers) => Task.FromResult(responseHandler(status, responseBody, headers)), CancellationToken.None);
            }

            public Task<T> Post<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, MultipartFormDataObject multipart, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, T> responseHandler)
            {
                return Post(uri, requestHeaders, multipart, (status, body, headers) => Task.FromResult(responseHandler(status, body, headers)), CancellationToken.None);
            }

            public Task<T> Put<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, string body, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, T> responseHandler)
            {
                return Put(uri, requestHeaders, body, (status, responseBody, headers) => Task.FromResult(responseHandler(status, responseBody, headers)), CancellationToken.None);
            }

            public Task<T> Put<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, MultipartFormDataObject multipart, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, T> responseHandler)
            {
                return Put(uri, requestHeaders, multipart, (status, body, headers) => Task.FromResult(responseHandler(status, body, headers)), CancellationToken.None);
            }

            public Task<T> Get<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, Task<T>> responseHandler, CancellationToken cancellationToken)
            {
                return Respond("GET", requestHeaders, false, responseHandler, cancellationToken);
            }

            public Task<T> Delete<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, Task<T>> responseHandler, CancellationToken cancellationToken)
            {
                return Respond("DELETE", requestHeaders, false, responseHandler, cancellationToken);
            }

            public Task<T> Post<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, string body, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, Task<T>> responseHandler, CancellationToken cancellationToken)
            {
                return Respond("POST", requestHeaders, false, responseHandler, cancellationToken);
            }

            public Task<T> Post<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, MultipartFormDataObject multipart, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, Task<T>> responseHandler, CancellationToken cancellationToken)
            {
                return Respond("POST", requestHeaders, true, responseHandler, cancellationToken);
            }

            public Task<T> Put<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, string body, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, Task<T>> responseHandler, CancellationToken cancellationToken)
            {
                return Respond("PUT", requestHeaders, false, responseHandler, cancellationToken);
            }

            public Task<T> Put<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, MultipartFormDataObject multipart, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, Task<T>> responseHandler, CancellationToken cancellationToken)
            {
                return Respond("PUT", requestHeaders, true, responseHandler, cancellationToken);
            }
        }

        private abstract class FakeAuthenticator : IAuthenticator
        {
            public int Calls { get; protected set; }

            public List<CancellationToken> Tokens { get; } = new List<CancellationToken>();

            public CancellationToken? LastToken => Tokens.Count == 0 ? (CancellationToken?)null : Tokens[Tokens.Count - 1];

            public abstract string Authorization { get; }

            public Task<string> GetAuthorization(HttpMethod httpMethod, Uri resourceUri, IEnumerable<IRequestHeader> requestHeaders)
            {
                Calls++;
                return Task.FromResult(Authorization);
            }
        }

        // only implements IAuthenticator, like every authenticator that existed before cancellation support
        private sealed class PlainAuthenticator : FakeAuthenticator
        {
            public override string Authorization => "Bearer plain";
        }

        private sealed class CancellableAuthenticator : FakeAuthenticator, ICancellableAuthenticator
        {
            public override string Authorization => "Bearer cancellable";

            public Task<string> GetAuthorization(HttpMethod httpMethod, Uri resourceUri, IEnumerable<IRequestHeader> requestHeaders, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Calls++;
                Tokens.Add(cancellationToken);
                return Task.FromResult(Authorization);
            }
        }
    }
}
