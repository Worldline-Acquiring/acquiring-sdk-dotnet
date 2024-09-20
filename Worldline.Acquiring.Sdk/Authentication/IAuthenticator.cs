using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Communication;

namespace Worldline.Acquiring.Sdk.Authentication
{
    /// <summary>
    /// Used to authenticate requests to the Worldline Acquiring platform. Thread-safe.
    /// </summary>
    public interface IAuthenticator
    {
        /// <summary>
        /// Returns a value that can be used for the <c>Authorization</c> header.
        /// </summary>
        /// <returns>The <c>Authorization</c> header value.</returns>
        /// <param name="httpMethod">HTTP method.</param>
        /// <param name="resourceUri">Resource URI.</param>
        /// <param name="requestHeaders">A list of request headers.</param>
        /// <remarks>The list of Request headers may not be modified and may not contain headers with the same name.</remarks>
        Task<string> GetAuthorization(HttpMethod httpMethod, Uri resourceUri, IEnumerable<IRequestHeader> requestHeaders);
    }
}
