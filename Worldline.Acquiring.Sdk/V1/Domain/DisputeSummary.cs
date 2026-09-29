/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DisputeSummary
    {
        /// <summary>
        /// The reference provided by the acquirer for the dispute.
        /// </summary>
        public string AcquirerDisputeReference { get; set; }

        /// <summary>
        /// Summary data related to the merchant involved in the dispute.
        /// </summary>
        public DisputeMerchantDataBase MerchantData { get; set; }

        /// <summary>
        /// Data related to the original transaction that led to the dispute.
        /// </summary>
        public OriginalTransactionSummaryData OriginalTransactionData { get; set; }

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
