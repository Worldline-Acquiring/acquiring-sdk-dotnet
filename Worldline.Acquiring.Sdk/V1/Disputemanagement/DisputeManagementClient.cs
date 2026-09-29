/*
 * This file was automatically generated.
 */
using System.Collections.Generic;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.V1.Domain;

namespace Worldline.Acquiring.Sdk.V1.Disputemanagement
{
    /// <summary>
    /// DisputeManagement client. Thread-safe.
    /// </summary>
    public class DisputeManagementClient : ApiResource
    {
        public DisputeManagementClient(ApiResource parent, IDictionary<string, string> pathContext) :
            base(parent, pathContext)
        {
        }

        /// <summary>
        /// Resource /dispute-management/v1/disputes/search
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Dispute-Management/operation/searchDisputes">Search Disputes</a>
        /// </summary>
        /// <param name="body">SearchDisputesRequest</param>
        /// <param name="context">CallContext</param>
        /// <returns>SearchDisputesResponse</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task<SearchDisputesResponse> SearchDisputes(SearchDisputesRequest body, CallContext context = null)
        {
            var uri = InstantiateUri("/dispute-management/v1/disputes/search", null);
            try
            {
                return await _communicator.Post<SearchDisputesResponse>(
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
        /// Resource /dispute-management/v1/disputes/{disputeId}
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Dispute-Management/operation/getDispute">Retrieve Dispute</a>
        /// </summary>
        /// <param name="disputeId">string</param>
        /// <param name="query">GetDisputeParams</param>
        /// <param name="context">CallContext</param>
        /// <returns>DisputeResponse</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task<DisputeResponse> GetDispute(string disputeId, GetDisputeParams query, CallContext context = null)
        {
            var pathContext = new Dictionary<string, string>
            {
                { "disputeId", disputeId }
            };
            var uri = InstantiateUri("/dispute-management/v1/disputes/{disputeId}", pathContext);
            try
            {
                return await _communicator.Get<DisputeResponse>(
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
        /// Resource /dispute-management/v1/disputes/{disputeId}/accept
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Dispute-Management/operation/acceptDisputeLiability">Accept Liability</a>
        /// </summary>
        /// <param name="disputeId">string</param>
        /// <param name="body">AcceptDisputeLiabilityRequest</param>
        /// <param name="context">CallContext</param>
        /// <returns>DisputeResponse</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task<DisputeResponse> AcceptDisputeLiability(string disputeId, AcceptDisputeLiabilityRequest body, CallContext context = null)
        {
            var pathContext = new Dictionary<string, string>
            {
                { "disputeId", disputeId }
            };
            var uri = InstantiateUri("/dispute-management/v1/disputes/{disputeId}/accept", pathContext);
            try
            {
                return await _communicator.Post<DisputeResponse>(
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
        /// Resource /dispute-management/v1/disputes/{disputeId}/submit-evidence
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Dispute-Management/operation/submitEvidence">Submit Evidence</a>
        /// </summary>
        /// <param name="disputeId">string</param>
        /// <param name="body">SubmitEvidenceRequest</param>
        /// <param name="context">CallContext</param>
        /// <returns>DisputeResponse</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task<DisputeResponse> SubmitEvidence(string disputeId, SubmitEvidenceRequest body, CallContext context = null)
        {
            var pathContext = new Dictionary<string, string>
            {
                { "disputeId", disputeId }
            };
            var uri = InstantiateUri("/dispute-management/v1/disputes/{disputeId}/submit-evidence", pathContext);
            try
            {
                return await _communicator.Post<DisputeResponse>(
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
