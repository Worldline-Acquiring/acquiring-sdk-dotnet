/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DisputeEntryWithDisputeSummary : DisputeEntry
    {
        /// <summary>
        /// The unique identifier for a dispute.
        /// </summary>
        public string DisputeId { get; set; }

        /// <summary>
        /// A summary of the dispute case, returned in the search results when searching for dispute<br />
        /// entries. This is only returned when explicitly requested via the <c>includeDisputeSummary</c> property is set to <c>true</c>
        /// in the request.
        /// </summary>
        public DisputeSummary DisputeSummary { get; set; }
    }
}
