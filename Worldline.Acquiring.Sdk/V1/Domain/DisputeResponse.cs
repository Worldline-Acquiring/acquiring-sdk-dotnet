/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DisputeResponse
    {
        /// <summary>
        /// A dispute represents a chargeback case. It contains details on the dispute and a
        /// history of entries that were done against the dispute during the lifecycle of the dispute.
        /// <p />
        /// Each dispute has a unique <c>disputeId</c> that identifies it. You can use this <c>disputeId</c> to
        /// retrieve the details of the dispute and its history via the <a href="#operation/getDispute">Retrieve Dispute</a> endpoint,
        /// or to retrieve specific documents related to the dispute via the <a href="#operation/getDisputeDocument">Retrieve Dispute Document</a>
        /// endpoint.
        /// <p />
        /// We return both the <c>schemeReason</c>, which is the reason provided by the card scheme for the
        /// dispute, and the <c>unifiedReason</c>, which is our mapping of the scheme reason to a unified reason
        /// that we use across all schemes. This allows you to easily filter disputes based on the
        /// <c>unifiedReason</c>, while still having the <c>schemeReason</c> available for reference. The
        /// <c>unifiedCategory</c> is a high level category of the dispute reason, that we use to group similar
        /// <c>unifiedReason</c> values together.
        /// <p />
        /// The <c>disputeStatus</c> field represents the current status of the dispute. The possible values
        /// for this field are specific to each card scheme, but we also provide a <c>disputeStatusCategory</c>
        /// field that groups the different <c>disputeStatus</c> values into a few high level categories that
        /// are consistent across all schemes. This allows you to easily filter disputes based on the
        /// <c>disputeStatusCategory</c>, while still having the <c>disputeStatus</c> available for reference.
        /// <p />
        /// The <c>disputeStage</c> field represents the current stage of the dispute in the dispute lifecycle.
        /// The possible values for this field represent the different stages that a dispute can be in during
        /// its lifecycle, such as &quot;DISPUTE&quot;, &quot;PRE_ARBITRATION&quot;, &quot;ARBITRATION&quot;, etc.
        /// <p />
        /// The <c>originalDisputeAmount</c> object represents the original amount of the dispute when it was first
        /// created. This amount can change during the lifecycle of the dispute, for example if the cardholder
        /// disputes only part of the original transaction amount, or if there are fees applied to the dispute.
        /// The <c>merchantBalanceAmount</c> field represents the current amount that's charged to or credited
        /// back to the merchant for this dispute. This amount can be different from the <c>originalDisputeAmount</c>
        /// due to partial disputes, fees, or if the dispute was challenged by the merchant and is currently
        /// being reviewed by the card scheme.
        /// <p />
        /// The <c>disputeReferences</c> object contains a set of references related to the dispute, such as the
        /// acquirer dispute reference and the scheme dispute reference. These references can be used when
        /// communicating with the acquirer or the card scheme about the dispute.
        /// <p />
        /// The <c>DisputeDateTimeData</c> object contains a set of date time fields related to the
        /// dispute, such as the opened date, response due date, closed date, etc. These fields can provide more
        /// context on the timeline of the dispute. When the <c>disputeStatus</c> is &quot;EVIDENCE_REQUESTED&quot;, the
        /// <c>responseDueDate</c> field indicates the deadline to respond to the dispute.
        /// <p />
        /// The <c>originalTransactionData</c> object contains data related to the original transaction that led to the
        /// dispute, such as transaction references, transaction amount, payment method data, etc. This information
        /// can be useful to understand the context of the dispute and to provide evidence when challenging the
        /// dispute.
        /// <p />
        /// The <c>merchantData</c> object contains data related to the merchant involved in the dispute, such as merchant
        /// name, merchant category code, acquirer ID, etc. This information can also be useful to understand the
        /// context of the dispute and to provide evidence when challenging the dispute.
        /// <p />
        /// Optionally the full history of entries for the dispute is included by setting the <c>includeEntries</c>
        /// parameter to <c>true</c> when retrieving the dispute details. Each entry in the history represents a step
        /// in the lifecycle of the dispute, such as when the dispute was opened, when evidence was requested by the
        /// acquirer, when evidence was provided by the merchant, when a credit adjustment was made by the acquirer,
        /// etc. The history is ordered from the oldest entry to the most recent one.
        /// </summary>
        public DisputeCaseWithEntries Dispute { get; set; }

        /// <summary>
        /// The unique Worldline identifier for the request that resulted in this response.
        /// </summary>
        public string RequestId { get; set; }
    }
}
