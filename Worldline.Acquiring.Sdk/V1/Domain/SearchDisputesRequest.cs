/*
 * This file was automatically generated.
 */
using System.Collections.Generic;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class SearchDisputesRequest
    {
        /// <summary>
        /// The reference provided by the acquirer for the dispute.
        /// </summary>
        public string AcquirerDisputeReference { get; set; }

        /// <summary>
        /// Acquirer reference number (ARN) for transaction
        /// </summary>
        public string AcquirerReferenceNumber { get; set; }

        /// <summary>
        /// A range of date time values, used to select disputes that have a date time field that falls within this range.
        /// <p />
        /// <b>NOTE</b>: You can set either one or both of the lower and greater than properties to filter the results.
        /// </summary>
        public DateTimeRange ClosedDateTime { get; set; }

        /// <summary>
        /// The unique identifier for a dispute.
        /// </summary>
        public string DisputeId { get; set; }

        public IList<string> DisputeStages { get; set; }

        public IList<string> DisputeStatusCategories { get; set; }

        /// <summary>
        /// Indicates whether the dispute is open or closed. An open dispute is a dispute that's still in the process
        /// of being resolved, while a closed dispute is a dispute that has been resolved.
        /// </summary>
        public bool? IsOpen { get; set; }

        /// <summary>
        /// A range of date time values, used to select disputes that have a date time field that falls within this range.
        /// <p />
        /// <b>NOTE</b>: You can set either one or both of the lower and greater than properties to filter the results.
        /// </summary>
        public DateTimeRange LastStatusChangedDateTime { get; set; }

        /// <summary>
        /// Reference for the transaction to allow the merchant to reconcile their payments in our report files
        /// and in their disputes.<br />
        /// It is advised to submit a unique value per transaction.<br />
        /// The value is returned in the baseTrxType/addlMercData element of the MRX file.
        /// </summary>
        public string MerchantReference { get; set; }

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
        /// A range of date time values, used to select disputes that have a date time field that falls within this range.
        /// <p />
        /// <b>NOTE</b>: You can set either one or both of the lower and greater than properties to filter the results.
        /// </summary>
        public DateTimeRange OpenedDateTime { get; set; }

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
        /// The unique identifier for the original payment transaction that resulted in the dispute.
        /// Depending on the interface used for the original transaction different values are returned. If the original
        /// transaction was made through the Acquiring API, the <c>paymentId</c> from the original transaction is returned.
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// A range of date values, used to select disputes that have a date field that falls within this range.
        /// <p />
        /// <b>NOTE</b>: You can set either one or both of the properties to filter the results.
        /// </summary>
        public DateRange ResponseDueDate { get; set; }

        public IList<string> Schemes { get; set; }

        /// <summary>
        /// The field by which to sort the search results. This can be any of the date-time fields in
        /// the dispute resource, such as <c>openedDateTime</c>, <c>closedDateTime</c>, <c>lastStatusChangedDateTime</c> or
        /// <c>responseDueDate</c>.
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// The order in which to sort the search results. Can be either ascending (ASC) or descending (DESC).
        /// </summary>
        public string SortOrder { get; set; }

        public IList<string> UnifiedCategories { get; set; }
    }
}
