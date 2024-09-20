/*
 * This file was automatically generated.
 */
using System.Collections.Generic;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.V1.Domain;

namespace Worldline.Acquiring.Sdk.V1.Ping
{
    /// <summary>
    /// Ping client. Thread-safe.
    /// </summary>
    public class PingClient : ApiResource
    {
        public PingClient(ApiResource parent, IDictionary<string, string> pathContext) :
            base(parent, pathContext)
        {
        }

        /// <summary>
        /// Resource /services/v1/ping
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Ping/operation/ping">Check API connection</a>
        /// </summary>
        /// <param name="context">CallContext</param>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task Ping(CallContext context = null)
        {
            var uri = InstantiateUri("/services/v1/ping", null);
            try
            {
                await _communicator.Get<object>(
                        uri,
                        null,
                        null,
                        context).ConfigureAwait(false);
            }
            catch (ResponseException e)
            {
                object errorObject = _communicator.Marshaller.Unmarshal<ApiPaymentErrorResponse>(e.Body);
                throw ExceptionFactory.CreateException(e.StatusCode, e.Body, errorObject, context);
            }
        }
    }
}
