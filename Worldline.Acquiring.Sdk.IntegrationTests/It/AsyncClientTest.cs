using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Authentication;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.Domain;
using Worldline.Acquiring.Sdk.Json;
using Worldline.Acquiring.Sdk.Util;
using Worldline.Acquiring.Sdk.V1.Disputedocuments;
using MockResponse = Worldline.Acquiring.Sdk.Util.MockHttpServer.MockResponse;
using RecordedRequest = Worldline.Acquiring.Sdk.Util.MockHttpServer.RecordedRequest;

namespace Worldline.Acquiring.Sdk.It
{
    /// <summary>
    /// Verifies the <c>...Async</c> methods of the generated clients for the two operations that are not plain JSON calls:
    /// the multipart/form-data upload and the binary download with an asynchronous body handler.
    /// It uses a local <see cref="MockHttpServer"/> and a stub authenticator, so these tests do not depend on any external service or credentials.
    /// </summary>
    [TestFixture]
    public class AsyncClientTest
    {
        private const string Authorization = "Bearer test-token";
        private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(5);

        private MockHttpServer _server;

        [SetUp]
        public void SetUp()
        {
            _server = new MockHttpServer();
        }

        [TearDown]
        public void TearDown()
        {
            _server.Dispose();
        }

        [TestCase]
        public async Task TestUploadDisputeDocumentAsync()
        {
            _server.Enqueue(MockResponse.Json(200, "{\"documentId\":\"document-1\",\"requestId\":\"request-1\"}"));

            using (var communicator = CreateCommunicator())
            using (var cts = new CancellationTokenSource(RequestTimeout))
            {
                var client = Factory.CreateClient(communicator);
                var request = new UploadDisputeDocumentRequest
                {
                    File = new UploadableFile("evidence.txt", new MemoryStream(Encoding.UTF8.GetBytes("evidence-content")), "text/plain")
                };

                var response = await client.V1.DisputeDocuments.UploadDisputeDocumentAsync(request, cancellationToken: cts.Token);

                Assert.That(response, Is.Not.Null);
                Assert.That(response.DocumentId, Is.EqualTo("document-1"));
                Assert.That(response.RequestId, Is.EqualTo("request-1"));
            }

            AssertUploadRequest(TakeRequest());
        }

        [TestCase]
        public async Task TestUploadDisputeDocumentWithTheMethodWithoutTheSuffix()
        {
            _server.Enqueue(MockResponse.Json(200, "{\"documentId\":\"document-1\",\"requestId\":\"request-1\"}"));

            using (var communicator = CreateCommunicator())
            {
                var client = Factory.CreateClient(communicator);
                var request = new UploadDisputeDocumentRequest
                {
                    File = new UploadableFile("evidence.txt", new MemoryStream(Encoding.UTF8.GetBytes("evidence-content")), "text/plain")
                };

                var response = await client.V1.DisputeDocuments.UploadDisputeDocument(request);

                Assert.That(response.DocumentId, Is.EqualTo("document-1"));
            }

            AssertUploadRequest(TakeRequest());
        }

        [TestCase]
        public async Task TestGetDisputeDocumentAsyncWithAnAsynchronousBodyHandler()
        {
            _server.Enqueue(MockResponse.Json(200, "document-content"));

            string content = null;
            var handlerSawHeaders = false;
            using (var communicator = CreateCommunicator())
            using (var cts = new CancellationTokenSource(RequestTimeout))
            {
                var client = Factory.CreateClient(communicator);

                await client.V1.DisputeDocuments.GetDisputeDocumentAsync("dispute-1", "document-1", async (stream, headers) =>
                {
                    handlerSawHeaders = headers.Any(header => string.Equals(header.Name, "Content-Type", StringComparison.OrdinalIgnoreCase));

                    await Task.Delay(100);
                    using var buffer = new MemoryStream();
                    await stream.CopyToAsync(buffer);
                    content = Encoding.UTF8.GetString(buffer.ToArray());
                }, cancellationToken: cts.Token);
            }

            Assert.That(content, Is.EqualTo("document-content"));
            Assert.That(handlerSawHeaders, Is.True, "the handler did not receive the response headers");

            var recordedRequest = TakeRequest();
            Assert.That(recordedRequest.Method, Is.EqualTo("GET"));
            Assert.That(recordedRequest.Path, Is.EqualTo("/dispute-management/v1/disputes/dispute-1/documents/document-1"));
            Assert.That(recordedRequest.GetHeader("Authorization"), Is.EqualTo(Authorization));
        }

        [TestCase]
        public async Task TestGetDisputeDocumentWithTheSynchronousBodyHandler()
        {
            _server.Enqueue(MockResponse.Json(200, "document-content"));

            string content = null;
            using (var communicator = CreateCommunicator())
            {
                var client = Factory.CreateClient(communicator);

                await client.V1.DisputeDocuments.GetDisputeDocument("dispute-1", "document-1", (stream, headers) =>
                {
                    using var reader = new StreamReader(stream, Encoding.UTF8);
                    content = reader.ReadToEnd();
                });
            }

            Assert.That(content, Is.EqualTo("document-content"));
        }

        [TestCase]
        public async Task TestCancellingWhileTheAsynchronousBodyHandlerIsRunning()
        {
            // the response has arrived, so only the handler is left; cancelling the token must reach the caller as it is,
            // and must not be reported as a failure of the body handler
            _server.Enqueue(MockResponse.Json(200, "document-content"));

            var handlerStarted = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            using var communicator = CreateCommunicator();
            using var cts = new CancellationTokenSource();
            var client = Factory.CreateClient(communicator);

            var call = client.V1.DisputeDocuments.GetDisputeDocumentAsync("dispute-1", "document-1", async (stream, headers) =>
            {
                handlerStarted.TrySetResult(true);
                await Task.Delay(Timeout.Infinite, cts.Token);
            }, cancellationToken: cts.Token);

            await WithTimeout(handlerStarted.Task);
            Assert.That(call.IsCompleted, Is.False, "the handler is still running, so the call must still be in flight");

            cts.Cancel();

            var exception = Assert.CatchAsync<OperationCanceledException>(async () => await WithTimeout(call));
            Assert.That(exception, Is.Not.InstanceOf<CommunicationException>());
            Assert.That(exception, Is.Not.InstanceOf<BodyHandlerException>());
        }

        private static void AssertUploadRequest(RecordedRequest recordedRequest)
        {
            Assert.That(recordedRequest.Method, Is.EqualTo("POST"));
            Assert.That(recordedRequest.Path, Is.EqualTo("/dispute-management/v1/documents"));
            Assert.That(recordedRequest.GetHeader("Authorization"), Is.EqualTo(Authorization));
            Assert.That(recordedRequest.IsMultipart, Is.True, "the request is not a multipart/form-data request");

            var parts = recordedRequest.GetMultipartParts();
            Assert.That(parts.Count, Is.EqualTo(1));
            Assert.That(parts.TryGetValue("file", out var part), Is.True, "missing part: file");
            Assert.That(part.FileName, Is.EqualTo("evidence.txt"));
            Assert.That(part.MediaType, Is.EqualTo("text/plain"));
            Assert.That(part.ContentAsString, Is.EqualTo("evidence-content"));
        }

        private RecordedRequest TakeRequest()
        {
            var recordedRequest = _server.TakeRequest(RequestTimeout);
            Assert.That(recordedRequest, Is.Not.Null, "no request was received by the mock server");
            return recordedRequest;
        }

        private Communicator CreateCommunicator()
        {
            return new CommunicatorBuilder()
                .WithApiEndpoint(new Uri(_server.Url))
                .WithConnection(new DefaultConnection(TimeSpan.FromSeconds(10)))
                .WithAuthenticator(new StubAuthenticator())
                .WithMetadataProvider(new MetadataProvider("Worldline"))
                .WithMarshaller(DefaultMarshaller.Instance)
                .Build();
        }

        private static async Task WithTimeout(Task task)
        {
            var finished = await Task.WhenAny(task, Task.Delay(RequestTimeout));
            if (finished != task)
            {
                throw new TimeoutException("the call did not complete");
            }
            await task;
        }

        private sealed class StubAuthenticator : IAuthenticator
        {
            public Task<string> GetAuthorization(HttpMethod httpMethod, Uri resourceUri, IEnumerable<IRequestHeader> requestHeaders)
            {
                return Task.FromResult(Authorization);
            }
        }
    }
}
