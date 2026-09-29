/*
 * This file was automatically generated.
 */
using System.Collections.Generic;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class SearchDisputeEntriesRequest
    {
        /// <summary>
        /// The unique identifier for a dispute.
        /// </summary>
        public string DisputeId { get; set; }

        public IList<string> EntryCategories { get; set; }

        /// <summary>
        /// A range of date time values, used to select disputes that have a date time field that falls within this range.
        /// <p />
        /// <b>NOTE</b>: You can set either one or both of the lower and greater than properties to filter the results.
        /// </summary>
        public DateTimeRange EntryDateTime { get; set; }

        /// <summary>
        /// The unique identifier for an dispute entry.
        /// </summary>
        public string EntryId { get; set; }

        public IList<string> EntryTypes { get; set; }

        /// <summary>
        /// If true, the summary of the dispute case will be included in the response for dispute entries search.
        /// The dispute case summary includes key information about the dispute case such as the current status,
        /// the reason for the dispute and the amount of the disputed transaction. This can be useful to provide context
        /// about the dispute case when searching for specific dispute entries.<br />
        /// False by default.
        /// </summary>
        public bool? IncludeDisputeSummary { get; set; }

        /// <summary>
        /// A set of fields to specify the scope of the search for disputes related to a specific merchant or set of merchants.
        /// The following options are available:
        /// <list type="bullet">
        ///   <item><description>Search for disputes related to specific acquirers, by providing the <c>acquirerIds</c> field.</description></item>
        ///   <item><description>Search for disputes related to specific merchant groups, by providing the <c>merchantRootIds</c> field.</description></item>
        ///   <item><description>Search for disputes related to specific merchants, by providing the <c>merchantIds</c> field.</description></item>
        /// </list>
        /// <p />
        /// If no merchant scope is provided, disputes for all merchants that the API user has access to will be returned (that
        /// also match the other provided search criteria, if any).
        /// </summary>
        public MerchantScope MerchantScope { get; set; }

        /// <summary>
        /// Pagination details for paginated responses.
        /// <p />
        /// First request: Omit <c>searchId</c>, set <c>fromIndex</c>=0, <c>pageSize</c>=20
        /// Subsequent requests: Use <c>searchId</c> from previous response to maintain query context.
        /// <p />
        /// Note: <c>searchId</c> expires after 24 hours. If you should perform your original search request again.
        /// </summary>
        public PaginationRequest Pagination { get; set; }

        /// <summary>
        /// The order in which to sort the dispute entries in the response. Can be either ascending (ASC) or descending (DESC). The sorting is done based on the <c>entryDateTime</c> field of the dispute entries, so when the sort order is ascending, the oldest entry is returned first and the newest entry is returned last. By default, the dispute entries are sorted in descending order.
        /// </summary>
        public string SortOrder { get; set; }
    }
}
