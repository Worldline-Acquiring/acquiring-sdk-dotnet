using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Authentication;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.Domain;
using Worldline.Acquiring.Sdk.Json;
using Worldline.Acquiring.Sdk.Util;
using EchoResponse = Worldline.Acquiring.Sdk.Util.MockHttpServer.EchoResponse;
using MockResponse = Worldline.Acquiring.Sdk.Util.MockHttpServer.MockResponse;
using MultipartPart = Worldline.Acquiring.Sdk.Util.MockHttpServer.MultipartPart;
using RecordedRequest = Worldline.Acquiring.Sdk.Util.MockHttpServer.RecordedRequest;

namespace Worldline.Acquiring.Sdk.It
{
    /// <summary>
    /// Verifies the multipart/form-data requests that the SDK puts on the wire, using a local <see cref="MockHttpServer"/>.
    /// The communicator uses a stub authenticator, so these tests do not depend on any external service or credentials.
    /// </summary>
    [TestFixture]
    public class MultipartFormDataTest
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
        public async Task TestPostMultipartFormDataObjectWithResponse()
        {
            using (var communicator = CreateCommunicator())
            {
                var response = await communicator.Post<EchoResponse>("/post", null, null, CreateFileAndValueMultipart(), null, CancellationToken.None);

                AssertFileAndValueEchoed(response);
            }

            AssertFileAndValueRequest(TakeRequest(), "POST", "/post");
        }

        [TestCase]
        public async Task TestPostMultipartFormDataObjectWithBodyHandler()
        {
            var handled = false;

            using (var communicator = CreateCommunicator())
            {
                await communicator.Post("/post", null, null, CreateFileAndValueMultipart(), (stream, _) =>
                {
                    AssertFileAndValueEchoed(DefaultMarshaller.Instance.Unmarshal<EchoResponse>(stream));
                    handled = true;
                    return Task.CompletedTask;
                }, null, CancellationToken.None);
            }

            Assert.IsTrue(handled, "body handler was not called");
            AssertFileAndValueRequest(TakeRequest(), "POST", "/post");
        }

        [TestCase]
        public async Task TestPostIMultipartFormDataRequestWithResponse()
        {
            using (var communicator = CreateCommunicator())
            {
                var request = new MultipartFormDataObjectWrapper(CreateFileAndValueMultipart());
                var response = await communicator.Post<EchoResponse>("/post", null, null, request, null, CancellationToken.None);

                AssertFileAndValueEchoed(response);
            }

            AssertFileAndValueRequest(TakeRequest(), "POST", "/post");
        }

        [TestCase]
        public async Task TestPostIMultipartFormDataRequestWithBodyHandler()
        {
            var handled = false;

            using (var communicator = CreateCommunicator())
            {
                var request = new MultipartFormDataObjectWrapper(CreateFileAndValueMultipart());
                await communicator.Post("/post", null, null, request, (stream, _) =>
                {
                    AssertFileAndValueEchoed(DefaultMarshaller.Instance.Unmarshal<EchoResponse>(stream));
                    handled = true;
                    return Task.CompletedTask;
                }, null, CancellationToken.None);
            }

            Assert.IsTrue(handled, "body handler was not called");
            AssertFileAndValueRequest(TakeRequest(), "POST", "/post");
        }

        // PUT, single file and value

        [TestCase]
        public async Task TestPutMultipartFormDataObjectWithResponse()
        {
            using (var communicator = CreateCommunicator())
            {
                var response = await communicator.Put<EchoResponse>("/put", null, null, CreateFileAndValueMultipart(), null, CancellationToken.None);

                AssertFileAndValueEchoed(response);
            }

            AssertFileAndValueRequest(TakeRequest(), "PUT", "/put");
        }

        [TestCase]
        public async Task TestPutMultipartFormDataObjectWithBodyHandler()
        {
            var handled = false;

            using (var communicator = CreateCommunicator())
            {
                await communicator.Put("/put", null, null, CreateFileAndValueMultipart(), (stream, _) =>
                {
                    AssertFileAndValueEchoed(DefaultMarshaller.Instance.Unmarshal<EchoResponse>(stream));
                    handled = true;
                    return Task.CompletedTask;
                }, null, CancellationToken.None);
            }

            Assert.IsTrue(handled, "body handler was not called");
            AssertFileAndValueRequest(TakeRequest(), "PUT", "/put");
        }

        [TestCase]
        public async Task TestPutIMultipartFormDataRequestWithResponse()
        {
            using (var communicator = CreateCommunicator())
            {
                var request = new MultipartFormDataObjectWrapper(CreateFileAndValueMultipart());
                var response = await communicator.Put<EchoResponse>("/put", null, null, request, null, CancellationToken.None);

                AssertFileAndValueEchoed(response);
            }

            AssertFileAndValueRequest(TakeRequest(), "PUT", "/put");
        }

        [TestCase]
        public async Task TestPutIMultipartFormDataRequestWithBodyHandler()
        {
            var handled = false;

            using (var communicator = CreateCommunicator())
            {
                var request = new MultipartFormDataObjectWrapper(CreateFileAndValueMultipart());
                await communicator.Put("/put", null, null, request, (stream, _) =>
                {
                    AssertFileAndValueEchoed(DefaultMarshaller.Instance.Unmarshal<EchoResponse>(stream));
                    handled = true;
                    return Task.CompletedTask;
                }, null, CancellationToken.None);
            }

            Assert.IsTrue(handled, "body handler was not called");
            AssertFileAndValueRequest(TakeRequest(), "PUT", "/put");
        }

        [TestCase]
        public async Task TestPostTwoFiles()
        {
            var multipart = new MultipartFormDataObject();
            multipart.AddFile("firstFile", new UploadableFile("first.txt", CreateStream("firstContent"), "text/plain"));
            multipart.AddFile("secondFile", new UploadableFile("second.txt", CreateStream("secondContent"), "text/plain"));

            var parts = await PostAndTakeParts(multipart);

            Assert.AreEqual(2, parts.Count);
            AssertFilePart(parts, "firstFile", "first.txt", "text/plain", "firstContent");
            AssertFilePart(parts, "secondFile", "second.txt", "text/plain", "secondContent");
        }

        [TestCase]
        public async Task TestPostThreeFilesWithDifferentTypes()
        {
            var multipart = new MultipartFormDataObject();
            multipart.AddFile("textFile", new UploadableFile("file.txt", CreateStream("text"), "text/plain"));
            multipart.AddFile("jsonFile", new UploadableFile("file.json", CreateStream("{\"key\": \"json\"}"), "application/json"));
            multipart.AddFile("xmlFile", new UploadableFile("file.xml", CreateStream("<key>xml</key>"), "application/xml"));

            var parts = await PostAndTakeParts(multipart);

            Assert.AreEqual(3, parts.Count);
            AssertFilePart(parts, "textFile", "file.txt", "text/plain", "text");
            AssertFilePart(parts, "jsonFile", "file.json", "application/json", "{\"key\": \"json\"}");
            AssertFilePart(parts, "xmlFile", "file.xml", "application/xml", "<key>xml</key>");
        }

        [TestCase]
        public async Task TestPostTwoValues()
        {
            var multipart = new MultipartFormDataObject();
            multipart.AddValue("firstKey", "firstValue");
            multipart.AddValue("secondKey", "secondValue");

            var parts = await PostAndTakeParts(multipart);

            Assert.AreEqual(2, parts.Count);
            AssertValuePart(parts, "firstKey", "firstValue");
            AssertValuePart(parts, "secondKey", "secondValue");
        }

        [TestCase]
        public async Task TestPostThreeValues()
        {
            var multipart = new MultipartFormDataObject();
            multipart.AddValue("name", "John");
            multipart.AddValue("age", "30");
            multipart.AddValue("city", "NYC");

            var parts = await PostAndTakeParts(multipart);

            Assert.AreEqual(3, parts.Count);
            AssertValuePart(parts, "name", "John");
            AssertValuePart(parts, "age", "30");
            AssertValuePart(parts, "city", "NYC");
        }

        [TestCase]
        public async Task TestPostNonAsciiValue()
        {
            var multipart = new MultipartFormDataObject();
            multipart.AddValue("greeting", "Hello, Šđčćž €");

            var parts = await PostAndTakeParts(multipart);

            Assert.AreEqual(1, parts.Count);
            AssertValuePart(parts, "greeting", "Hello, Šđčćž €");
        }

        [TestCase]
        public async Task TestPostSingleFileWithoutValues()
        {
            var multipart = new MultipartFormDataObject();
            multipart.AddFile("document", new UploadableFile("doc.pdf", CreateStream("doc content"), "application/pdf"));

            var parts = await PostAndTakeParts(multipart);

            Assert.AreEqual(1, parts.Count);
            AssertFilePart(parts, "document", "doc.pdf", "application/pdf", "doc content");
        }

        [TestCase]
        public async Task TestPostSingleValueWithoutFiles()
        {
            var multipart = new MultipartFormDataObject();
            multipart.AddValue("message", "Hello");

            var parts = await PostAndTakeParts(multipart);

            Assert.AreEqual(1, parts.Count);
            AssertValuePart(parts, "message", "Hello");
        }

        [TestCase("pdf", "document.pdf", "application/pdf", "pdf content")]
        [TestCase("image", "photo.jpg", "image/jpeg", "image content")]
        [TestCase("data", "data.json", "application/json", "{\"key\": \"value\"}")]
        public async Task TestPostFileWithContentType(string parameterName, string fileName, string contentType, string content)
        {
            var multipart = new MultipartFormDataObject();
            multipart.AddFile(parameterName, new UploadableFile(fileName, CreateStream(content), contentType));

            var parts = await PostAndTakeParts(multipart);

            Assert.AreEqual(1, parts.Count);
            AssertFilePart(parts, parameterName, fileName, contentType, content);
        }

        [TestCase]
        public async Task TestPostBinaryFile()
        {
            var content = new byte[64 * 1024];
            for (var i = 0; i < content.Length; i++)
            {
                content[i] = (byte)i;
            }
            var multipart = new MultipartFormDataObject();
            multipart.AddFile("binary", new UploadableFile("data.bin", new MemoryStream(content), "application/octet-stream"));

            var parts = await PostAndTakeParts(multipart);

            Assert.AreEqual(1, parts.Count);
            AssertFilePart(parts, "binary", "data.bin", "application/octet-stream", content);
        }

        // content length

        [TestCase]
        public async Task TestPostSendsContentLengthForFileWithKnownLength()
        {
            var multipart = new MultipartFormDataObject();
            multipart.AddFile("file", new UploadableFile("file.txt", CreateStream("content"), "text/plain", 7));

            using (var communicator = CreateCommunicator())
            {
                var response = await communicator.Post<EchoResponse>("/post", null, null, multipart, null, CancellationToken.None);
                Assert.NotNull(response);
            }

            var recordedRequest = TakeRequest();
            Assert.IsNull(recordedRequest.GetHeader("Transfer-Encoding"));
            Assert.AreEqual(recordedRequest.Body.Length.ToString(), recordedRequest.GetHeader("Content-Length"));

            var parts = AssertMultipartRequest(recordedRequest, "POST", "/post");
            Assert.AreEqual(1, parts.Count);
            AssertFilePart(parts, "file", "file.txt", "text/plain", "content");
        }

        [TestCase]
        public async Task TestPostUsesChunkedTransferForFileWithUnknownLength()
        {
            var multipart = new MultipartFormDataObject();
            multipart.AddFile("file", new UploadableFile("file.txt", new NonSeekableStream(CreateStream("content")), "text/plain"));

            using (var communicator = CreateCommunicator())
            {
                var response = await communicator.Post<EchoResponse>("/post", null, null, multipart, null, CancellationToken.None);
                Assert.NotNull(response);
            }

            var recordedRequest = TakeRequest();
            Assert.AreEqual(recordedRequest.GetHeader("Transfer-Encoding").ToLower(), "chunked");
            Assert.IsNull(recordedRequest.GetHeader("Content-Length"));

            var parts = AssertMultipartRequest(recordedRequest, "POST", "/post");
            Assert.AreEqual(1, parts.Count);
            AssertFilePart(parts, "file", "file.txt", "text/plain", "content");
        }

        // error response

        [TestCase]
        public Task TestPostThrowsResponseExceptionForJsonErrorResponse()
        {
            const string errorBody = "{\"status\": 400, \"title\": \"Bad Request\"}";
            _server.Enqueue(MockResponse.Json(400, errorBody));

            var multipart = new MultipartFormDataObject();
            multipart.AddFile("file", new UploadableFile("file.txt", CreateStream("file-content"), "text/plain"));

            using (var communicator = CreateCommunicator())
            {
                var exception = Assert.ThrowsAsync<ResponseException>(
                    async () => await communicator.Post<EchoResponse>("/post", null, null, multipart, null, CancellationToken.None));

                if (exception != null)
                {
                    Assert.AreEqual(HttpStatusCode.BadRequest, exception.StatusCode);
                    Assert.AreEqual(errorBody, exception.Body);
                }
            }

            var parts = AssertMultipartRequest(TakeRequest(), "POST", "/post");
            Assert.AreEqual(1, parts.Count);
            AssertFilePart(parts, "file", "file.txt", "text/plain", "file-content");

            return Task.CompletedTask;
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

        private async Task<IReadOnlyDictionary<string, MultipartPart>> PostAndTakeParts(MultipartFormDataObject multipart)
        {
            using (var communicator = CreateCommunicator())
            {
                var response = await communicator.Post<EchoResponse>("/post", null, null, multipart, null, CancellationToken.None);
                Assert.NotNull(response);
            }

            return AssertMultipartRequest(TakeRequest(), "POST", "/post");
        }

        private static MultipartFormDataObject CreateFileAndValueMultipart()
        {
            var multipart = new MultipartFormDataObject();
            multipart.AddFile("file", new UploadableFile("file.txt", CreateStream("file-content"), "text/plain"));
            multipart.AddValue("value", "Hello World");
            return multipart;
        }

        private static Stream CreateStream(string content)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        private RecordedRequest TakeRequest()
        {
            var recordedRequest = _server.TakeRequest(RequestTimeout);
            Assert.NotNull(recordedRequest, "no request was received by the mock server");
            return recordedRequest;
        }

        private static void AssertFileAndValueEchoed(EchoResponse response)
        {
            Assert.NotNull(response);

            Assert.NotNull(response.Form);
            Assert.AreEqual(1, response.Form.Count);
            Assert.AreEqual("Hello World", response.Form["value"]);

            Assert.NotNull(response.Files);
            Assert.AreEqual(1, response.Files.Count);
            Assert.AreEqual("file-content", response.Files["file"]);
        }

        private static void AssertFileAndValueRequest(RecordedRequest recordedRequest, string expectedMethod, string expectedPath)
        {
            var parts = AssertMultipartRequest(recordedRequest, expectedMethod, expectedPath);
            Assert.AreEqual(2, parts.Count);
            AssertFilePart(parts, "file", "file.txt", "text/plain", "file-content");
            AssertValuePart(parts, "value", "Hello World");
        }

        /// <summary>
        /// Verifies the request line and headers of a multipart/form-data request, and returns its parts keyed by parameter name.
        /// </summary>
        private static IReadOnlyDictionary<string, MultipartPart> AssertMultipartRequest(RecordedRequest recordedRequest, string expectedMethod, string expectedPath)
        {
            Assert.AreEqual(expectedMethod, recordedRequest.Method);
            Assert.AreEqual(expectedPath, recordedRequest.Path);
            Assert.AreEqual(Authorization, recordedRequest.GetHeader("Authorization"));

            var contentType = recordedRequest.GetHeader("Content-Type");
            Assert.NotNull(contentType, "Content-Type header is missing");
            Assert.That(contentType.ToLower(), Does.StartWith("multipart/form-data; boundary="));

            return recordedRequest.GetMultipartParts();
        }

        private static void AssertFilePart(IReadOnlyDictionary<string, MultipartPart> parts, string name, string fileName, string contentType, string content)
        {
            AssertFilePart(parts, name, fileName, contentType, Encoding.UTF8.GetBytes(content));
        }

        private static void AssertFilePart(IReadOnlyDictionary<string, MultipartPart> parts, string name, string fileName, string contentType, byte[] content)
        {
            Assert.IsTrue(parts.TryGetValue(name, out var part), "missing part: " + name);
            Assert.AreEqual(fileName, part.FileName);
            Assert.AreEqual(contentType, part.MediaType);
            Assert.AreEqual(content, part.Content);
        }

        private static void AssertValuePart(IReadOnlyDictionary<string, MultipartPart> parts, string name, string value)
        {
            Assert.IsTrue(parts.TryGetValue(name, out var part), "missing part: " + name);
            Assert.IsNull(part.FileName, "value part should not have a filename");
            Assert.AreEqual("text/plain", part.MediaType);
            Assert.AreEqual("utf-8", part.Charset.ToLower());
            Assert.AreEqual(value, part.ContentAsString);
        }

        private sealed class StubAuthenticator : IAuthenticator
        {
            public Task<string> GetAuthorization(HttpMethod httpMethod, Uri resourceUri, IEnumerable<IRequestHeader> requestHeaders)
            {
                return Task.FromResult(Authorization);
            }
        }

        private sealed class MultipartFormDataObjectWrapper(MultipartFormDataObject multipart)
            : IMultipartFormDataRequest
        {
            public MultipartFormDataObject ToMultipartFormDataObject()
            {
                return multipart;
            }
        }

        /// <summary>
        /// Wraps a stream and hides its length, like a network or pipe stream would.
        /// </summary>
        private sealed class NonSeekableStream(Stream @delegate) : Stream
        {
            public override bool CanRead => true;
            public override bool CanSeek => false;
            public override bool CanWrite => false;
            public override long Length => throw new NotSupportedException();

            public override long Position
            {
                get => throw new NotSupportedException();
                set => throw new NotSupportedException();
            }

            public override int Read(byte[] buffer, int offset, int count) => @delegate.Read(buffer, offset, count);
            public override void Flush() { }
            public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
            public override void SetLength(long value) => throw new NotSupportedException();
            public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    @delegate.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }
}
