/*
 * This file was automatically generated.
 */
using System.Collections.Generic;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.V1.Domain;

namespace Worldline.Acquiring.Sdk.V1.Acquirer.Merchant.Refunds
{
    /// <summary>
    /// Refunds client. Thread-safe.
    /// </summary>
    public class RefundsClient : ApiResource
    {
        public RefundsClient(ApiResource parent, IDictionary<string, string> pathContext) :
            base(parent, pathContext)
        {
        }

        /// <summary>
        /// Resource /processing/v1/{acquirerId}/{merchantId}/refunds
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Refunds/operation/processStandaloneRefund">Create standalone refund</a>
        /// </summary>
        /// <param name="body">ApiRefundRequest</param>
        /// <param name="context">CallContext</param>
        /// <returns>ApiRefundResponse</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task<ApiRefundResponse> ProcessStandaloneRefund(ApiRefundRequest body, CallContext context = null)
        {
            var uri = InstantiateUri("/processing/v1/{acquirerId}/{merchantId}/refunds", null);
            try
            {
                return await _communicator.Post<ApiRefundResponse>(
                        uri,
                        null,
                        null,
                        body,
                        context).ConfigureAwait(false);
            }
            catch (ResponseException e)
            {
                object errorObject = _communicator.Marshaller.Unmarshal<ApiPaymentErrorResponse>(e.Body);
                throw ExceptionFactory.CreateException(e.StatusCode, e.Body, errorObject, context);
            }
        }

        /// <summary>
        /// Resource /processing/v1/{acquirerId}/{merchantId}/refunds/{refundId}
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Refunds/operation/getRefund">Retrieve refund</a>
        /// </summary>
        /// <param name="refundId">string</param>
        /// <param name="query">GetRefundParams</param>
        /// <param name="context">CallContext</param>
        /// <returns>ApiRefundResource</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task<ApiRefundResource> GetRefund(string refundId, GetRefundParams query, CallContext context = null)
        {
            var pathContext = new Dictionary<string, string>
            {
                { "refundId", refundId }
            };
            var uri = InstantiateUri("/processing/v1/{acquirerId}/{merchantId}/refunds/{refundId}", pathContext);
            try
            {
                return await _communicator.Get<ApiRefundResource>(
                        uri,
                        null,
                        query,
                        context).ConfigureAwait(false);
            }
            catch (ResponseException e)
            {
                object errorObject = _communicator.Marshaller.Unmarshal<ApiPaymentErrorResponse>(e.Body);
                throw ExceptionFactory.CreateException(e.StatusCode, e.Body, errorObject, context);
            }
        }

        /// <summary>
        /// Resource /processing/v1/{acquirerId}/{merchantId}/refunds/{refundId}/captures
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Refunds/operation/captureRefund">Capture refund</a>
        /// </summary>
        /// <param name="refundId">string</param>
        /// <param name="body">ApiCaptureRequestForRefund</param>
        /// <param name="context">CallContext</param>
        /// <returns>ApiActionResponseForRefund</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task<ApiActionResponseForRefund> CaptureRefund(string refundId, ApiCaptureRequestForRefund body, CallContext context = null)
        {
            var pathContext = new Dictionary<string, string>
            {
                { "refundId", refundId }
            };
            var uri = InstantiateUri("/processing/v1/{acquirerId}/{merchantId}/refunds/{refundId}/captures", pathContext);
            try
            {
                return await _communicator.Post<ApiActionResponseForRefund>(
                        uri,
                        null,
                        null,
                        body,
                        context).ConfigureAwait(false);
            }
            catch (ResponseException e)
            {
                object errorObject = _communicator.Marshaller.Unmarshal<ApiPaymentErrorResponse>(e.Body);
                throw ExceptionFactory.CreateException(e.StatusCode, e.Body, errorObject, context);
            }
        }

        /// <summary>
        /// Resource /processing/v1/{acquirerId}/{merchantId}/refunds/{refundId}/authorization-reversals
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Refunds/operation/reverseRefundAuthorization">Reverse refund authorization</a>
        /// </summary>
        /// <param name="refundId">string</param>
        /// <param name="body">ApiPaymentReversalRequest</param>
        /// <param name="context">CallContext</param>
        /// <returns>ApiActionResponseForRefund</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task<ApiActionResponseForRefund> ReverseRefundAuthorization(string refundId, ApiPaymentReversalRequest body, CallContext context = null)
        {
            var pathContext = new Dictionary<string, string>
            {
                { "refundId", refundId }
            };
            var uri = InstantiateUri("/processing/v1/{acquirerId}/{merchantId}/refunds/{refundId}/authorization-reversals", pathContext);
            try
            {
                return await _communicator.Post<ApiActionResponseForRefund>(
                        uri,
                        null,
                        null,
                        body,
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
