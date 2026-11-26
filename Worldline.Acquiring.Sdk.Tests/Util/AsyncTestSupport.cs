using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Worldline.Acquiring.Sdk.Util
{
    /// <summary>
    /// An <see cref="HttpMessageHandler"/> that lets a test decide what happens to a request, without any network.
    /// </summary>
    internal sealed class DelegateHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _send;

        internal DelegateHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> send)
        {
            _send = send;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _send(request, cancellationToken);
        }
    }

    /// <summary>
    /// A minimal HTTP server on the loopback interface that either answers every request with a fixed JSON body, or never answers at all.
    /// It uses a plain <see cref="TcpListener"/>, so unlike <c>HttpListener</c> it needs no URL reservation or administrator rights.
    /// </summary>
    internal sealed class StubHttpServer : IDisposable
    {
        private readonly TcpListener _listener;
        private readonly CancellationTokenSource _stop = new CancellationTokenSource();
        private readonly TaskCompletionSource<bool> _firstRequest = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        private int _requestCount;

        /// <param name="responseBody">The JSON body to answer each request with, or <c>null</c> to never answer.</param>
        internal StubHttpServer(string responseBody)
        {
            ResponseBody = responseBody;
            _listener = new TcpListener(IPAddress.Loopback, 0);
            _listener.Start();
            Task.Run(AcceptLoop);
        }

        /// <summary>
        /// The body to answer with. Can be changed while the server is running; requests that are already waiting keep waiting.
        /// </summary>
        internal string ResponseBody { get; set; }

        internal Uri BaseUri => new Uri("http://127.0.0.1:" + ((IPEndPoint)_listener.LocalEndpoint).Port + "/");

        /// <summary>
        /// The number of requests that were completely received.
        /// </summary>
        internal int RequestCount => Volatile.Read(ref _requestCount);

        /// <summary>
        /// Completes when the first request was completely received.
        /// </summary>
        internal Task FirstRequestReceived => _firstRequest.Task;

        private async Task AcceptLoop()
        {
            while (!_stop.IsCancellationRequested)
            {
                TcpClient client;
                try
                {
                    client = await _listener.AcceptTcpClientAsync().ConfigureAwait(false);
                }
                catch (Exception)
                {
                    // the listener was stopped
                    return;
                }
                _ = Task.Run(() => Handle(client));
            }
        }

        private async Task Handle(TcpClient client)
        {
            using (client)
            {
                try
                {
                    var stream = client.GetStream();
                    await ReadRequest(stream).ConfigureAwait(false);
                    Interlocked.Increment(ref _requestCount);
                    _firstRequest.TrySetResult(true);

                    var body = ResponseBody;
                    if (body == null)
                    {
                        await Task.Delay(Timeout.Infinite, _stop.Token).ConfigureAwait(false);
                        return;
                    }

                    var bodyBytes = Encoding.UTF8.GetBytes(body);
                    var head = "HTTP/1.1 200 OK\r\nContent-Type: application/json\r\nContent-Length: " + bodyBytes.Length + "\r\nConnection: close\r\n\r\n";
                    var headBytes = Encoding.ASCII.GetBytes(head);
                    await stream.WriteAsync(headBytes, 0, headBytes.Length).ConfigureAwait(false);
                    await stream.WriteAsync(bodyBytes, 0, bodyBytes.Length).ConfigureAwait(false);
                    await stream.FlushAsync().ConfigureAwait(false);
                }
                catch (Exception)
                {
                    // the client went away, or the server was stopped
                }
            }
        }

        // Reads the request headers, and the body if there is one, so the client has sent everything before the stub answers or hangs
        private static async Task ReadRequest(NetworkStream stream)
        {
            var received = new StringBuilder();
            var buffer = new byte[4096];
            int headerEnd;
            while ((headerEnd = received.ToString().IndexOf("\r\n\r\n", StringComparison.Ordinal)) < 0)
            {
                var read = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                if (read == 0)
                {
                    return;
                }
                received.Append(Encoding.Latin1.GetString(buffer, 0, read));
            }

            var contentLength = 0;
            foreach (var line in received.ToString(0, headerEnd).Split(new[] { "\r\n" }, StringSplitOptions.None))
            {
                if (line.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase))
                {
                    contentLength = int.Parse(line.Substring("Content-Length:".Length).Trim());
                }
            }

            var bodyReceived = received.Length - (headerEnd + 4);
            while (bodyReceived < contentLength)
            {
                var read = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                if (read == 0)
                {
                    return;
                }
                bodyReceived += read;
            }
        }

        public void Dispose()
        {
            _stop.Cancel();
            try
            {
                _listener.Stop();
            }
            catch (Exception)
            {
                // ignore
            }
            _stop.Dispose();
        }
    }
}
