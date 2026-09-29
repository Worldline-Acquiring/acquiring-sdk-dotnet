/*
 * This file was automatically generated.
 */
using System.Collections.Generic;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class SearchDisputesResponse
    {
        /// <summary>
        /// A list of dispute cases matching the provided search criteria. Each dispute case contains
        /// the full details of the dispute, but without the full history of entries for the dispute.
        /// These can be retrieved by setting the <c>includeEntries</c> parameter to true when using the
        /// <a href="#operation/getDispute">Retrieve Dispute</a> endpoint.
        /// </summary>
        public IList<DisputeCase> Disputes { get; set; }

        /// <summary>
        /// Pagination details for paginated responses.
        /// </summary>
        public PaginationResponse Pagination { get; set; }

        /// <summary>
        /// The unique Worldline identifier for the request that resulted in this response.
        /// </summary>
        public string RequestId { get; set; }
    }
}
