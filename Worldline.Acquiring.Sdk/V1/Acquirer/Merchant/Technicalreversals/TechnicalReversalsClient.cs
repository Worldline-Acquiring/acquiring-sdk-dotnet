/*
 * This file was automatically generated.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.V1.Domain;

namespace Worldline.Acquiring.Sdk.V1.Acquirer.Merchant.Technicalreversals
{
    /// <summary>
    /// TechnicalReversals client. Thread-safe.
    /// </summary>
    public class TechnicalReversalsClient : ApiResource
    {
        public TechnicalReversalsClient(ApiResource parent, IDictionary<string, string> pathContext) :
            base(parent, pathContext)
        {
        }

        /// <summary>
        /// Resource /processing/v1/{acquirerId}/{merchantId}/operations/{operationId}/reverse
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Technical-Reversals/operation/technicalReversal">Technical reversal</a>
        /// </summary>
        /// <param name="operationId">string</param>
        /// <param name="body">ApiTechnicalReversalRequest</param>
        /// <param name="context">CallContext</param>
        /// <returns>ApiTechnicalReversalResponse</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        [Obsolete("Use TechnicalReversalAsync instead. This method will be removed in a future major version.")]
        public Task<ApiTechnicalReversalResponse> TechnicalReversal(string operationId, ApiTechnicalReversalRequest body, CallContext context = null)
        {
            return TechnicalReversalAsync(operationId, body, context, CancellationToken.None);
        }

        /// <summary>
        /// Resource /processing/v1/{acquirerId}/{merchantId}/operations/{operationId}/reverse
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Technical-Reversals/operation/technicalReversal">Technical reversal</a>
        /// </summary>
        /// <param name="operationId">string</param>
        /// <param name="body">ApiTechnicalReversalRequest</param>
        /// <param name="context">CallContext</param>
        /// <param name="cancellationToken">A token to cancel the call</param>
        /// <returns>ApiTechnicalReversalResponse</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        public async Task<ApiTechnicalReversalResponse> TechnicalReversalAsync(string operationId, ApiTechnicalReversalRequest body, CallContext context = null, CancellationToken cancellationToken = default)
        {
            var pathContext = new Dictionary<string, string>
            {
                { "operationId", operationId }
            };
            var uri = InstantiateUri("/processing/v1/{acquirerId}/{merchantId}/operations/{operationId}/reverse", pathContext);
            try
            {
                return await _communicator.Post<ApiTechnicalReversalResponse>(
                        uri,
                        null,
                        null,
                        body,
                        context,
                        cancellationToken).ConfigureAwait(false);
            }
            catch (ResponseException e)
            {
                object errorObject = _communicator.Marshaller.Unmarshal<ApiPaymentErrorResponse>(e.Body);
                throw ExceptionFactory.CreateException(e.StatusCode, e.Body, errorObject, context);
            }
        }
    }
}
