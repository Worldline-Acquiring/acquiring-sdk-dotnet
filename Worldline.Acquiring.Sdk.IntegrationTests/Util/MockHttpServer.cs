using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Json;

namespace Worldline.Acquiring.Sdk.Util;

/// <summary>
/// Simple in-process HTTP server for testing multipart requests.
/// <para>
/// Every request is recorded and can be retrieved using <see cref="TakeRequest"/>.
/// Requests are answered with the next response queued using <see cref="Enqueue"/>, or with an <see cref="EchoResponse"/> if no response is queued.
/// </para>
/// </summary>
public sealed class MockHttpServer : IDisposable
{
    private const int MaxStartAttempts = 5;

    private readonly HttpListener _listener;
    private readonly int _port;
    private readonly BlockingCollection<RecordedRequest> _requests = new();
    private readonly ConcurrentQueue<MockResponse> _responses = new();
    private readonly CancellationTokenSource _cancellation = new();

    public string Url => $"http://localhost:{_port}";

    public MockHttpServer()
    {
        (_listener, _port) = StartListener();
        _ = Task.Run(AcceptLoop);
    }

    /// <summary>
    /// Queues a response for the next request that is received.
    /// </summary>
    public void Enqueue(MockResponse response)
    {
        _responses.Enqueue(response ?? throw new ArgumentNullException(nameof(response)));
    }

    /// <summary>
    /// Returns the next received request, or <c>null</c> if no request was received within the given timeout.
    /// </summary>
    public RecordedRequest TakeRequest(TimeSpan timeout)
    {
        return _requests.TryTake(out var request, timeout) ? request : null;
    }

    private async Task AcceptLoop()
    {
        while (!_cancellation.IsCancellationRequested)
        {
            HttpListenerContext ctx;
            try
            {
                ctx = await _listener.GetContextAsync().ConfigureAwait(false);
            }
            catch (Exception e) when (e is HttpListenerException || e is ObjectDisposedException || e is InvalidOperationException)
            {
                // the listener was stopped
                break;
            }

            _ = Task.Run(() => HandleRequest(ctx));
        }
    }

    private async Task HandleRequest(HttpListenerContext ctx)
    {
        try
        {
            byte[] body;
            using (var buffer = new MemoryStream())
            {
                await ctx.Request.InputStream.CopyToAsync(buffer).ConfigureAwait(false);
                body = buffer.ToArray();
            }

            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var name in ctx.Request.Headers.AllKeys.Where(name => name != null))
            {
                headers[name] = string.Join(",", ctx.Request.Headers.GetValues(name) ?? []);
            }

            var request = new RecordedRequest(ctx.Request.HttpMethod, ctx.Request.RawUrl, headers, body);
            // record the request before responding, so it can be taken as soon as the client received the response
            _requests.Add(request);

            var response = _responses.TryDequeue(out var queued) ? queued : CreateEchoResponse(request);

            var responseBytes = Encoding.UTF8.GetBytes(response.Body ?? string.Empty);
            ctx.Response.StatusCode = response.StatusCode;
            ctx.Response.ContentType = response.ContentType;
            ctx.Response.ContentLength64 = responseBytes.Length;
            await ctx.Response.OutputStream.WriteAsync(responseBytes, 0, responseBytes.Length).ConfigureAwait(false);
            ctx.Response.Close();
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync($"MockHttpServer failed to handle request: {e}");
            try
            {
                ctx.Response.StatusCode = 500;
                ctx.Response.Close();
            }
            catch
            {
                // ignore; the connection is probably already gone
            }
        }
    }

    private static MockResponse CreateEchoResponse(RecordedRequest request)
    {
        var echo = new EchoResponse
        {
            Method = request.Method,
            Path = request.Path
        };

        if (request.IsMultipart)
        {
            echo.Form = new Dictionary<string, string>();
            echo.Files = new Dictionary<string, string>();
            foreach (var part in request.GetMultipartParts().Values)
            {
                if (part.FileName == null)
                {
                    echo.Form[part.Name] = part.ContentAsString;
                }
                else
                {
                    echo.Files[part.Name] = part.ContentAsString;
                }
            }
        }

        return MockResponse.Json(200, DefaultMarshaller.Instance.Marshal(echo));
    }

    private static (HttpListener, int) StartListener()
    {
        // another process may claim the free port before the listener is started, so retry a few times
        for (var attempt = 1; ; attempt++)
        {
            var port = GetFreePort();
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{port}/");
            try
            {
                listener.Start();
                return (listener, port);
            }
            catch (HttpListenerException) when (attempt < MaxStartAttempts)
            {
                listener.Close();
            }
        }
    }

    private static int GetFreePort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }

    public void Dispose()
    {
        _cancellation.Cancel();
        try
        {
            _listener.Stop();
            _listener.Close();
        }
        catch
        {
            // ignore
        }
        _cancellation.Dispose();
    }

    /// <summary>
    /// A response to return for a request.
    /// </summary>
    public sealed class MockResponse
    {
        public int StatusCode { get; private init; } = 200;
        public string ContentType { get; private init; } = "application/json";
        public string Body { get; private init; }

        public static MockResponse Json(int statusCode, string body)
        {
            return new MockResponse { StatusCode = statusCode, ContentType = "application/json", Body = body };
        }
    }

    /// <summary>
    /// The default response; echoes the request method and path, and for multipart requests the values and file contents.
    /// </summary>
    public sealed class EchoResponse
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public Dictionary<string, string> Form { get; set; }
        public Dictionary<string, string> Files { get; set; }
    }

    /// <summary>
    /// A request as it was received by the server.
    /// </summary>
    public sealed class RecordedRequest(
        string method,
        string path,
        IReadOnlyDictionary<string, string> headers,
        byte[] body)
    {
        private const string MultipartFormDataPrefix = "multipart/form-data";

        private IReadOnlyDictionary<string, string> Headers { get; } = headers;
        public string Method { get; } = method;
        public string Path { get; } = path;
        public byte[] Body { get; } = body;

        public string GetHeader(string name)
        {
            return Headers.GetValueOrDefault(name);
        }

        public bool IsMultipart => GetHeader("Content-Type")?.StartsWith(MultipartFormDataPrefix, StringComparison.OrdinalIgnoreCase) ?? false;

        /// <summary>
        /// Parses the body of a multipart/form-data request into its parts, keyed by parameter name.
        /// </summary>
        /// <exception cref="FormatException">if the body is not a valid multipart/form-data body</exception>
        public IReadOnlyDictionary<string, MultipartPart> GetMultipartParts()
        {
            var contentType = GetHeader("Content-Type") ?? throw new FormatException("Content-Type header is missing");
            var boundary = GetBoundary(contentType) ?? throw new FormatException("Content-Type has no boundary: " + contentType);

            // Latin-1 maps every byte to exactly one char, so binary content survives the round trip
            var body = Encoding.Latin1.GetString(Body);
            var delimiter = "--" + boundary;
            var closeDelimiter = delimiter + "--";
            if (!body.StartsWith(delimiter + "\r\n", StringComparison.Ordinal))
            {
                throw new FormatException("body does not start with the boundary delimiter");
            }
            var closeIndex = body.LastIndexOf(closeDelimiter, StringComparison.Ordinal);
            if (closeIndex < 0)
            {
                throw new FormatException("body does not contain the closing boundary delimiter");
            }

            var parts = new Dictionary<string, MultipartPart>(StringComparer.Ordinal);
            var segments = body.Substring(0, closeIndex).Split(delimiter);
            // segments[0] is the (empty) preamble before the first delimiter
            foreach (var segment in segments.Skip(1))
            {
                var part = MultipartPart.Parse(segment);
                if (!parts.TryAdd(part.Name, part))
                {
                    throw new FormatException("duplicate part: " + part.Name);
                }
            }
            return parts;
        }

        private static string GetBoundary(string contentType)
        {
            foreach (var element in contentType.Split(';'))
            {
                var trimmed = element.Trim();
                if (trimmed.StartsWith("boundary=", StringComparison.OrdinalIgnoreCase))
                {
                    return trimmed.Substring("boundary=".Length).Trim('"');
                }
            }
            return null;
        }
    }

    /// <summary>
    /// A single part of a multipart/form-data body.
    /// </summary>
    public sealed class MultipartPart
    {
        private static readonly Regex NamePattern = new("(?:^|;)\\s*name=\"?([^\";]*)\"?", RegexOptions.IgnoreCase);
        private static readonly Regex FileNamePattern = new("(?:^|;)\\s*filename=\"?([^\";]*)\"?", RegexOptions.IgnoreCase);

        private IReadOnlyDictionary<string, string> Headers { get; }
        public string Name { get; }
        public string FileName { get; }
        public byte[] Content { get; }

        public string ContentAsString => Encoding.UTF8.GetString(Content);

        /// <summary>The media type of the part's Content-Type header, without parameters.</summary>
        public string MediaType => GetHeader("Content-Type")?.Split(';')[0].Trim();

        /// <summary>The charset parameter of the part's Content-Type header, if any.</summary>
        public string Charset => GetHeader("Content-Type")?.Split(';').Skip(1)
            .Select(parameter => parameter.Trim())
            .Where(parameter => parameter.StartsWith("charset=", StringComparison.OrdinalIgnoreCase))
            .Select(parameter => parameter.Substring("charset=".Length).Trim('"'))
            .FirstOrDefault();

        private MultipartPart(string name, string fileName, IReadOnlyDictionary<string, string> headers, byte[] content)
        {
            Name = name;
            FileName = fileName;
            Headers = headers;
            Content = content;
        }

        private string GetHeader(string name)
        {
            return Headers.GetValueOrDefault(name);
        }

        /// <summary>
        /// Parses the text between two boundary delimiters: CRLF, headers, CRLF CRLF, content, CRLF.
        /// </summary>
        internal static MultipartPart Parse(string segment)
        {
            if (!segment.StartsWith("\r\n", StringComparison.Ordinal) || !segment.EndsWith("\r\n", StringComparison.Ordinal))
            {
                throw new FormatException("part is not enclosed in CRLF");
            }
            var rawPart = segment.Substring(2, segment.Length - 4);

            var headerEnd = rawPart.IndexOf("\r\n\r\n", StringComparison.Ordinal);
            if (headerEnd < 0)
            {
                throw new FormatException("part has no header separator");
            }

            var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var headerLine in rawPart.Substring(0, headerEnd).Split("\r\n"))
            {
                var colon = headerLine.IndexOf(':');
                if (colon <= 0)
                {
                    throw new FormatException("invalid part header: " + headerLine);
                }
                headers[headerLine[..colon].Trim()] = headerLine.Substring(colon + 1).Trim();
            }

            if (!headers.TryGetValue("Content-Disposition", out var contentDisposition))
            {
                throw new FormatException("part has no Content-Disposition header");
            }
            if (!contentDisposition.StartsWith("form-data", StringComparison.OrdinalIgnoreCase))
            {
                throw new FormatException("unexpected Content-Disposition: " + contentDisposition);
            }
            var nameMatch = NamePattern.Match(contentDisposition);
            if (!nameMatch.Success)
            {
                throw new FormatException("Content-Disposition has no name: " + contentDisposition);
            }
            var fileNameMatch = FileNamePattern.Match(contentDisposition);

            var content = Encoding.Latin1.GetBytes(rawPart.Substring(headerEnd + 4));
            return new MultipartPart(nameMatch.Groups[1].Value, fileNameMatch.Success ? fileNameMatch.Groups[1].Value : null, headers, content);
        }
    }
}
