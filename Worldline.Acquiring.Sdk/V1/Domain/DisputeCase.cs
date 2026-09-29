/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DisputeCase
    {
        /// <summary>
        /// A set of date time fields related to the dispute case. Depending on the <c>disputeStatus</c> and
        /// <c>disputeStage</c> of the dispute, different date time fields are relevant. For example, when the
        /// <c>disputeStatus</c> is &quot;EVIDENCE_REQUESTED&quot;, the <c>responseDueDate</c> field indicates the deadline to respond
        /// to the dispute.
        /// <p />
        /// The <c>closedDateTime</c> field represents the date and time when the dispute was closed. This field is only
        /// returned for closed disputes. Disputes that are open too long are automatically closed.
        /// </summary>
        public DisputeDateTimeData DisputeDateTimeData { get; set; }

        /// <summary>
        /// The unique identifier for a dispute.
        /// </summary>
        public string DisputeId { get; set; }

        /// <summary>
        /// A set of references related to the dispute case.
        /// </summary>
        public DisputeReferences DisputeReferences { get; set; }

        /// <summary>
        /// The current stage in the lifecycle of the dispute.
        /// <p />
        /// Possible values are:
        /// <list type="bullet">
        ///   <item><description>CREATED (The dispute case includes no dispute relevant information)</description></item>
        ///   <item><description>FRAUD (The dispute case is opened with a fraud report (Issuer cases only))</description></item>
        ///   <item><description>INQUIRY (Pre-dispute phase)</description></item>
        ///   <item><description>DISPUTE (The dispute case reached the dispute stage, which includes dispute and representment handling)</description></item>
        ///   <item><description>PRE_ARBITRATION (The dispute case is in the first stage of the case filing process, before escalation to arbitration)</description></item>
        ///   <item><description>ARBITRATION (The dispute case filing process escalated to the arbitration phase)</description></item>
        ///   <item><description>PRE_COMPLIANCE (The dispute case is in the first stage of the case filing process, before escalation to compliance)</description></item>
        ///   <item><description>COMPLIANCE (The dispute case filing process escalated to the compliance phase)</description></item>
        /// </list>
        /// </summary>
        public string DisputeStage { get; set; }

        /// <summary>
        /// The current status of the dispute.
        /// <p />
        /// Possible values are:
        /// <list type="bullet">
        ///   <item><description>For <c>IN_PROGRESS</c> dispute status category:
        ///     <list type="bullet">
        ///       <item><description>REVIEW_BY_ACQUIRER (Next action is on Acquirer side)</description></item>
        ///       <item><description>REVIEW_BY_ISSUER (Next action is on Issuer side)</description></item>
        ///       <item><description>REVIEW_BY_SCHEME (Next action is on Scheme side)</description></item>
        ///     </list>
        ///   </description></item>
        ///   <item><description>For <c>NEEDS_RESPONSE</c> dispute status category:
        ///     <list type="bullet">
        ///       <item><description>EVIDENCE_REQUESTED (Acquirer request evidence from merchant)</description></item>
        ///     </list>
        ///   </description></item>
        ///   <item><description>For <c>WON</c> dispute status category:
        ///     <list type="bullet">
        ///       <item><description>ISSUER_WITHDRAWN (Issuer withdraw the dispute and accept liability)</description></item>
        ///       <item><description>SUCCESSFUL_DEFENSE (Acquirer dispute defense was successful)</description></item>
        ///       <item><description>SCHEME_RULING (Dispute is escalated and scheme ruled in favor of Merchant)</description></item>
        ///     </list>
        ///   </description></item>
        ///   <item><description>For <c>LOST</c> dispute status category:
        ///     <list type="bullet">
        ///       <item><description>UNSUCCESSFUL_DEFENSE (Acquirer dispute defense was not successful)</description></item>
        ///       <item><description>UNANSWERED_EXPIRED (Dispute respond time expired, no further defense is possible)</description></item>
        ///       <item><description>ACCEPTED (Acquirer accepted liability)</description></item>
        ///       <item><description>SCHEME_RULING (Dispute is escalated and scheme ruled in favor of Issuer)</description></item>
        ///     </list>
        ///   </description></item>
        ///   <item><description>For <c>CANCELLED</c> dispute status category:
        ///     <list type="bullet">
        ///       <item><description>DISPUTE_CANCELLED (Issuer withdrew the dispute)</description></item>
        ///     </list>
        ///   </description></item>
        /// </list>
        /// </summary>
        public string DisputeStatus { get; set; }

        /// <summary>
        /// The category of the current status of the dispute.
        /// <p />
        /// Possible values are:
        /// <list type="bullet">
        ///   <item><description>IN_PROGRESS (Dispute process is ongoing and has no final decision. The merchant currently waits for further status updates.)</description></item>
        ///   <item><description>NEEDS_RESPONSE (The merchant is contacted to provide supporting evidence documents before a deadline)</description></item>
        ///   <item><description>WON (Dispute case has been won)</description></item>
        ///   <item><description>LOST (Dispute case has been lost)</description></item>
        ///   <item><description>CANCELLED (Dispute has been withdrawn)</description></item>
        /// </list>
        /// </summary>
        public string DisputeStatusCategory { get; set; }

        /// <summary>
        /// Indicates whether the dispute is open or closed. An open dispute is a dispute that's still in the process
        /// of being resolved, while a closed dispute is a dispute that has been resolved.
        /// </summary>
        public bool? IsOpen { get; set; }

        /// <summary>
        /// Amount with an indicator whether it's a debit or credit amount.
        /// </summary>
        public SignedAmountData MerchantBalanceAmount { get; set; }

        /// <summary>
        /// Data related to the merchant involved in the dispute.
        /// </summary>
        public DisputeMerchantData MerchantData { get; set; }

        /// <summary>
        /// Amount for the operation.
        /// </summary>
        public AmountData OriginalDisputeAmount { get; set; }

        /// <summary>
        /// Data related to the original transaction that led to the dispute.
        /// </summary>
        public OriginalTransactionData OriginalTransactionData { get; set; }

        /// <summary>
        /// The reason provided by the card scheme for the dispute.
        /// </summary>
        public string SchemeReason { get; set; }

        /// <summary>
        /// The human readable description of the reason provided by the card scheme for the dispute.
        /// </summary>
        public string SchemeReasonDescription { get; set; }

        /// <summary>
        /// The unified category of the dispute, used for categorization and reporting purposes.
        /// Only present if a dispute was received.
        /// <p />
        /// Possible values are:
        /// <list type="bullet">
        ///   <item><description>AUTHORIZATION_RELATED (authorization related disputes)</description></item>
        ///   <item><description>FRAUD_RELATED (fraud related disputes)</description></item>
        ///   <item><description>CONSUMER_DISPUTE (consumer initiated disputes)</description></item>
        ///   <item><description>PROCESSING_ERROR (error during the payment processing leads to a dispute)</description></item>
        ///   <item><description>OTHER (collection of diverse dispute reasons)</description></item>
        /// </list>
        /// </summary>
        public string UnifiedCategory { get; set; }

        /// <summary>
        /// The unified reason for the dispute, used for categorization and reporting purposes.
        /// Only present if a dispute was received.
        /// <p />
        /// Possible values are:
        /// <list type="bullet">
        ///   <item><description>FRAUDULENT_CARD_USAGE (Fraudulent use of card)</description></item>
        ///   <item><description>UNAUTHORIZED_TRANSACTION (No valid Issuer authorization)</description></item>
        ///   <item><description>GENERAL_PAYMENT_ERROR (General payment error)</description></item>
        ///   <item><description>INVALID_TRANSACTION_TYPE (Invalid transaction type)</description></item>
        ///   <item><description>INVALID_CURRENCY (Invalid currency)</description></item>
        ///   <item><description>INVALID_CARD_NUMBER (Invalid card number)</description></item>
        ///   <item><description>INVALID_AMOUNT (Invalid amount)</description></item>
        ///   <item><description>DUPLICATE_CHARGE_OR_PAID_BY_OTHER_MEANS (Duplicate charge or paid by other means)</description></item>
        ///   <item><description>GENERAL_CUSTOMER_DISPUTE (General customer dispute)</description></item>
        ///   <item><description>GOODS_OR_SERVICES_NOT_RECEIVED (Goods or services not received)</description></item>
        ///   <item><description>CANCELLED_SUBSCRIPTION (Cancelled subscription)</description></item>
        ///   <item><description>GOODS_OR_SERVICES_NOT_MATCHING_ORDER (Goods or services not matching order)</description></item>
        ///   <item><description>COUNTERFEIT_MERCHANDISE (Counterfeit merchandise)</description></item>
        ///   <item><description>DUE_REFUND_NOT_RECEIVED (Due refund not received)</description></item>
        ///   <item><description>CHARGE_NOT_ACCEPTED ((Subsequent) Charge not accepted)</description></item>
        ///   <item><description>OTHER (Other)</description></item>
        /// </list>
        /// </summary>
        public string UnifiedReason { get; set; }
    }
}
