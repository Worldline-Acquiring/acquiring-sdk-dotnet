using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Worldline.Acquiring.Sdk.Communication
{
    /// <summary>
    /// A connection to the Worldline Acquiring platform server that supports cancellation. Thread-safe.
    /// </summary>
    /// <remarks>
    /// The communicator uses this interface when the connection implements it, and otherwise falls back to <see cref="IConnection"/>,
    /// in which case a cancellation token is only honoured before the request is sent.
    /// A cancellation token that is cancelled after the response has arrived must not discard that response.
    /// The response handlers of this interface return a <see cref="Task{TResult}"/>, so they can await while the response is still open.
    /// There are no overloads with a synchronous response handler and a token, because those would make calls with a lambda ambiguous.
    /// </remarks>
    public interface ICancellableConnection : IConnection
    {
        /// <summary>
        /// Send a GET request to the Worldline Acquiring platform.
        /// </summary>
        /// <param name="uri">The URI to call, including any necessary query parameters.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="responseHandler">A callback that handles the stream from the response. The response stays open until the returned task has completed.</param>
        /// <param name="cancellationToken">A token to cancel the request.</param>
        /// <returns>Returns the object that was returned from the callback</returns>
        /// <exception cref="CommunicationException">when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        Task<T> Get<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, Task<T>> responseHandler, CancellationToken cancellationToken);

        /// <summary>
        /// Send a DELETE request to the Worldline Acquiring platform.
        /// </summary>
        /// <param name="uri">The URI to call, including any necessary query parameters.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="responseHandler">A callback that handles the stream from the response. The response stays open until the returned task has completed.</param>
        /// <param name="cancellationToken">A token to cancel the request.</param>
        /// <returns>Returns the object that was returned from the callback</returns>
        /// <exception cref="CommunicationException">when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        Task<T> Delete<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, Task<T>> responseHandler, CancellationToken cancellationToken);

        /// <summary>
        /// Send a POST request to the Worldline Acquiring platform.
        /// </summary>
        /// <param name="uri">The URI to call, including any necessary query parameters.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="body">The optional body to send.</param>
        /// <param name="responseHandler">A callback that handles the stream from the response. The response stays open until the returned task has completed.</param>
        /// <param name="cancellationToken">A token to cancel the request.</param>
        /// <returns>Returns the object that was returned from the callback</returns>
        /// <exception cref="CommunicationException">when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        Task<T> Post<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, string body, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, Task<T>> responseHandler, CancellationToken cancellationToken);

        /// <summary>
        /// Send a multipart/form-data POST request to the Worldline Acquiring platform.
        ///
        /// The content type of the request will be be part of the given request header list.
        /// If the connection creates its own content type, it should be multipart.getContentType().
        /// Otherwise, authentication failures will occur.
        /// </summary>
        /// <param name="uri">The URI to call, including any necessary query parameters.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="multipart">The multipart/form-data request to send.</param>
        /// <param name="responseHandler">A callback that handles the stream from the response. The response stays open until the returned task has completed.</param>
        /// <param name="cancellationToken">A token to cancel the request.</param>
        /// <returns>Returns the object that was returned from the callback</returns>
        /// <exception cref="CommunicationException">when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        Task<T> Post<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, MultipartFormDataObject multipart, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, Task<T>> responseHandler, CancellationToken cancellationToken);

        /// <summary>
        /// Send a PUT request to the Worldline Acquiring platform.
        /// </summary>
        /// <param name="uri">The URI to call, including any necessary query parameters.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="body">The optional body to send.</param>
        /// <param name="responseHandler">A callback that handles the stream from the response. The response stays open until the returned task has completed.</param>
        /// <param name="cancellationToken">A token to cancel the request.</param>
        /// <returns>Returns the object that was returned from the callback</returns>
        /// <exception cref="CommunicationException">when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        Task<T> Put<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, string body, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, Task<T>> responseHandler, CancellationToken cancellationToken);

        /// <summary>
        /// Send a multipart/form-data PUT request to the Worldline Acquiring platform.
        ///
        /// The content type of the request will be be part of the given request header list.
        /// If the connection creates its own content type, it should be multipart.getContentType().
        /// Otherwise, authentication failures will occur.
        /// </summary>
        /// <param name="uri">The URI to call, including any necessary query parameters.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="multipart">The multipart/form-data request to send.</param>
        /// <param name="responseHandler">A callback that handles the stream from the response. The response stays open until the returned task has completed.</param>
        /// <param name="cancellationToken">A token to cancel the request.</param>
        /// <returns>Returns the object that was returned from the callback</returns>
        /// <exception cref="CommunicationException">when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        Task<T> Put<T>(Uri uri, IEnumerable<IRequestHeader> requestHeaders, MultipartFormDataObject multipart, Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, Task<T>> responseHandler, CancellationToken cancellationToken);
    }
}
