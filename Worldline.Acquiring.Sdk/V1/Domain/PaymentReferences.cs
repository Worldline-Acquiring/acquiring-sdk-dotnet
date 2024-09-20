/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class PaymentReferences
    {
        /// <summary>
        /// Dynamic descriptor gives you the ability to control the descriptor
        /// on the credit card statement of the customer.
        /// </summary>
        public string DynamicDescriptor { get; set; }

        /// <summary>
        /// Reference for the transaction to allow the merchant to reconcile their payments in our report files.<br />
        /// It is advised to submit a unique value per transaction.<br />
        /// The value provided here is returned in the baseTrxType/addlMercData element of the MRX file.
        /// </summary>
        public string MerchantReference { get; set; }

        /// <summary>
        /// Retrieval reference number for transaction, must be AN(12) if provided
        /// </summary>
        public string RetrievalReferenceNumber { get; set; }
    }
}
