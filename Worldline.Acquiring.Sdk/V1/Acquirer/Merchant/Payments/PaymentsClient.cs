/*
 * This file was automatically generated.
 */
using System.Collections.Generic;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.V1.Domain;

namespace Worldline.Acquiring.Sdk.V1.Acquirer.Merchant.Payments
{
    /// <summary>
    /// Payments client. Thread-safe.
    /// </summary>
    public class PaymentsClient : ApiResource
    {
        public PaymentsClient(ApiResource parent, IDictionary<string, string> pathContext) :
            base(parent, pathContext)
        {
        }

        /// <summary>
        /// Resource /processing/v1/{acquirerId}/{merchantId}/payments
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Payments/operation/processPayment">Create payment</a>
        /// </summary>
        /// <param name="body">ApiPaymentRequest</param>
        /// <param name="context">CallContext</param>
        /// <returns>ApiPaymentResponse</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task<ApiPaymentResponse> ProcessPayment(ApiPaymentRequest body, CallContext context = null)
        {
            var uri = InstantiateUri("/processing/v1/{acquirerId}/{merchantId}/payments", null);
            try
            {
                return await _communicator.Post<ApiPaymentResponse>(
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
        /// Resource /processing/v1/{acquirerId}/{merchantId}/payments/{paymentId}
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Payments/operation/getPaymentStatus">Retrieve payment</a>
        /// </summary>
        /// <param name="paymentId">string</param>
        /// <param name="query">GetPaymentStatusParams</param>
        /// <param name="context">CallContext</param>
        /// <returns>ApiPaymentResource</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task<ApiPaymentResource> GetPaymentStatus(string paymentId, GetPaymentStatusParams query, CallContext context = null)
        {
            var pathContext = new Dictionary<string, string>
            {
                { "paymentId", paymentId }
            };
            var uri = InstantiateUri("/processing/v1/{acquirerId}/{merchantId}/payments/{paymentId}", pathContext);
            try
            {
                return await _communicator.Get<ApiPaymentResource>(
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
        /// Resource /processing/v1/{acquirerId}/{merchantId}/payments/{paymentId}/captures
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Payments/operation/simpleCaptureOfPayment">Capture payment</a>
        /// </summary>
        /// <param name="paymentId">string</param>
        /// <param name="body">ApiCaptureRequest</param>
        /// <param name="context">CallContext</param>
        /// <returns>ApiActionResponse</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task<ApiActionResponse> SimpleCaptureOfPayment(string paymentId, ApiCaptureRequest body, CallContext context = null)
        {
            var pathContext = new Dictionary<string, string>
            {
                { "paymentId", paymentId }
            };
            var uri = InstantiateUri("/processing/v1/{acquirerId}/{merchantId}/payments/{paymentId}/captures", pathContext);
            try
            {
                return await _communicator.Post<ApiActionResponse>(
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
        /// Resource /processing/v1/{acquirerId}/{merchantId}/payments/{paymentId}/authorization-reversals
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Payments/operation/reverseAuthorization">Reverse authorization</a>
        /// </summary>
        /// <param name="paymentId">string</param>
        /// <param name="body">ApiPaymentReversalRequest</param>
        /// <param name="context">CallContext</param>
        /// <returns>ApiReversalResponse</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task<ApiReversalResponse> ReverseAuthorization(string paymentId, ApiPaymentReversalRequest body, CallContext context = null)
        {
            var pathContext = new Dictionary<string, string>
            {
                { "paymentId", paymentId }
            };
            var uri = InstantiateUri("/processing/v1/{acquirerId}/{merchantId}/payments/{paymentId}/authorization-reversals", pathContext);
            try
            {
                return await _communicator.Post<ApiReversalResponse>(
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
        /// Resource /processing/v1/{acquirerId}/{merchantId}/payments/{paymentId}/increments
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Payments/operation/incrementPayment">Increment authorization</a>
        /// </summary>
        /// <param name="paymentId">string</param>
        /// <param name="body">ApiIncrementRequest</param>
        /// <param name="context">CallContext</param>
        /// <returns>ApiIncrementResponse</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task<ApiIncrementResponse> IncrementPayment(string paymentId, ApiIncrementRequest body, CallContext context = null)
        {
            var pathContext = new Dictionary<string, string>
            {
                { "paymentId", paymentId }
            };
            var uri = InstantiateUri("/processing/v1/{acquirerId}/{merchantId}/payments/{paymentId}/increments", pathContext);
            try
            {
                return await _communicator.Post<ApiIncrementResponse>(
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
        /// Resource /processing/v1/{acquirerId}/{merchantId}/payments/{paymentId}/refunds
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Payments/operation/createRefund">Refund payment</a>
        /// </summary>
        /// <param name="paymentId">string</param>
        /// <param name="body">ApiPaymentRefundRequest</param>
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
        public async Task<ApiActionResponseForRefund> CreateRefund(string paymentId, ApiPaymentRefundRequest body, CallContext context = null)
        {
            var pathContext = new Dictionary<string, string>
            {
                { "paymentId", paymentId }
            };
            var uri = InstantiateUri("/processing/v1/{acquirerId}/{merchantId}/payments/{paymentId}/refunds", pathContext);
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
