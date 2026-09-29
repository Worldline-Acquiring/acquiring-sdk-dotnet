/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class TransactionReferencesBase
    {
        /// <summary>
        /// Acquirer reference number (ARN) for transaction
        /// </summary>
        public string AcquirerReferenceNumber { get; set; }

        /// <summary>
        /// Reference for the transaction to allow the merchant to reconcile their payments in our report files
        /// and in their disputes.<br />
        /// It is advised to submit a unique value per transaction.<br />
        /// The value is returned in the baseTrxType/addlMercData element of the MRX file.
        /// </summary>
        public string MerchantReference { get; set; }

        /// <summary>
        /// The unique identifier for the original payment transaction that resulted in the dispute.
        /// Depending on the interface used for the original transaction different values are returned. If the original
        /// transaction was made through the Acquiring API, the <c>paymentId</c> from the original transaction is returned.
        /// </summary>
        public string PaymentId { get; set; }
    }
}
