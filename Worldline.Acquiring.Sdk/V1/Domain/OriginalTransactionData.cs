/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class OriginalTransactionData
    {
        /// <summary>
        /// The cardholder verification method (CVM) used in the original transaction that led to the dispute.
        /// </summary>
        public string CardholderVerificationMethod { get; set; }

        /// <summary>
        /// The local date and time of the original transaction capture that resulted in the dispute, in ISO 8601 format,
        /// but without the timezone designator.
        /// </summary>
        public string LocalTransactionDateTime { get; set; }

        /// <summary>
        /// The category of the payment used in the original transaction that led to the dispute.
        /// <p />
        /// Possible values are:
        /// <list type="bullet">
        ///   <item><description><c>SALES</c> (Different kind of payments with debits the recipient)</description></item>
        ///   <item><description><c>CREDIT_VOUCHER</c> (Refund/Credit payment)</description></item>
        ///   <item><description><c>ORIGINAL_CREDIT</c> (Transaction that credits the recipient in a payment transaction (money send, original credit) or cardholder funds transfer)</description></item>
        ///   <item><description><c>ATM</c> (ATM Deposit)</description></item>
        ///   <item><description><c>ACCOUNT_FUNDING</c> (Transaction that debits the sender in a payment transaction (money send, original credit))</description></item>
        ///   <item><description><c>CASH_ADVANCE</c> (Cash advance payment)</description></item>
        /// </list>
        /// </summary>
        public string PaymentCategory { get; set; }

        /// <summary>
        /// The payment method used in the original transaction that led to the dispute.
        /// </summary>
        public PaymentMethodData PaymentMethodData { get; set; }

        /// <summary>
        /// The point of sale (POS) entry mode used in the original transaction that led to the dispute.
        /// <p />
        /// Non-exclusive list of possible values:
        /// <list type="bullet">
        ///   <item><description>Unknown</description></item>
        ///   <item><description>Track1</description></item>
        ///   <item><description>Track2</description></item>
        ///   <item><description>Track3</description></item>
        ///   <item><description>Chip</description></item>
        ///   <item><description>Manual</description></item>
        ///   <item><description>Contactless-EMV</description></item>
        ///   <item><description>Contactless-Magstripe</description></item>
        ///   <item><description>Account ID</description></item>
        ///   <item><description>EMV fallback</description></item>
        ///   <item><description>Server or Wallet</description></item>
        ///   <item><description>QRC Code TAGC</description></item>
        ///   <item><description>CredentialOnFile</description></item>
        ///   <item><description>URL-intent</description></item>
        /// </list>
        /// </summary>
        public string PointOfSaleEntryMode { get; set; }

        /// <summary>
        /// The date and time when the dispute was processed by the card scheme, in ISO 8601 format,
        /// but without the timezone designator.
        /// </summary>
        public string SchemeProcessedDateTime { get; set; }

        /// <summary>
        /// Amount for the operation.
        /// </summary>
        public AmountData SettlementAmount { get; set; }

        /// <summary>
        /// Amount for the operation.
        /// </summary>
        public AmountData TransactionAmount { get; set; }

        /// <summary>
        /// A full set of references related to the original transaction that led to the dispute.
        /// </summary>
        public TransactionReferencesDispute TransactionReferences { get; set; }
    }
}
