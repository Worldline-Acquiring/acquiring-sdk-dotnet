using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Communication;

namespace Worldline.Acquiring.Sdk.Authentication
{
    /// <summary>
    /// An authenticator that supports cancellation. Thread-safe.
    /// </summary>
    /// <remarks>
    /// The communicator uses this interface when the authenticator implements it, and otherwise falls back to <see cref="IAuthenticator"/>,
    /// in which case a cancellation token is only honoured before the authorization is requested.
    /// </remarks>
    public interface ICancellableAuthenticator : IAuthenticator
    {
        /// <summary>
        /// Returns a value that can be used for the <c>Authorization</c> header.
        /// </summary>
        /// <returns>The <c>Authorization</c> header value.</returns>
        /// <param name="httpMethod">HTTP method.</param>
        /// <param name="resourceUri">Resource URI.</param>
        /// <param name="requestHeaders">A list of request headers.</param>
        /// <param name="cancellationToken">A token to cancel waiting for, or retrieving, the authorization.</param>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        /// <remarks>The list of Request headers may not be modified and may not contain headers with the same name.</remarks>
        Task<string> GetAuthorization(HttpMethod httpMethod, Uri resourceUri, IEnumerable<IRequestHeader> requestHeaders, CancellationToken cancellationToken);
    }
}
