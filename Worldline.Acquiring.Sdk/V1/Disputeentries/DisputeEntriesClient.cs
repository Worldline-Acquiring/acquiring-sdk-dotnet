/*
 * This file was automatically generated.
 */
using System.Collections.Generic;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.V1.Domain;

namespace Worldline.Acquiring.Sdk.V1.Disputeentries
{
    /// <summary>
    /// DisputeEntries client. Thread-safe.
    /// </summary>
    public class DisputeEntriesClient : ApiResource
    {
        public DisputeEntriesClient(ApiResource parent, IDictionary<string, string> pathContext) :
            base(parent, pathContext)
        {
        }

        /// <summary>
        /// Resource /dispute-management/v1/dispute-entries/search
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Dispute-Entries/operation/searchDisputeEntries">Search Dispute Entries</a>
        /// </summary>
        /// <param name="body">SearchDisputeEntriesRequest</param>
        /// <param name="context">CallContext</param>
        /// <returns>DisputeEntryResources</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task<DisputeEntryResources> SearchDisputeEntries(SearchDisputeEntriesRequest body, CallContext context = null)
        {
            var uri = InstantiateUri("/dispute-management/v1/dispute-entries/search", null);
            try
            {
                return await _communicator.Post<DisputeEntryResources>(
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
