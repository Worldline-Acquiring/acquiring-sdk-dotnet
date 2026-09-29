/*
 * This file was automatically generated.
 */
using System.Collections.Generic;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DisputeEntryResources
    {
        /// <summary>
        /// A list of dispute entries matching the provided search criteria. Each entry documents a single event in the
        /// lifecycle of the dispute, such as when the dispute was opened, when evidence was requested by the acquirer,
        /// when evidence was provided by the merchant, when a credit adjustment was made by the acquirer, etc.
        /// <p />
        /// Each entry has a category and type that indicate what kind of step it represents. The data elements can be
        /// different, depending on the entry type. For some steps more details are provided in a message text.
        /// <p />
        /// For each entry, if there are documents related to it, a list of document metadata will be included in the
        /// <c>documents</c> field.
        /// <p />
        /// Optionally a <c>disputeSummary</c> object can be included for each entry by setting the <c>includeDisputeSummary</c>
        /// parameter to <c>true</c> in the request of the <a href="#operation/searchDisputeEntries">Search Dispute Entries</a> endpoint. This summary contains key
        /// information on the dispute case that can be useful to understand the context of the entry, such as the
        /// current status and stage of the dispute, the type of dispute, the amount in dispute, the reason code, etc.
        /// </summary>
        public IList<DisputeEntryWithDisputeSummary> DisputeEntries { get; set; }

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
