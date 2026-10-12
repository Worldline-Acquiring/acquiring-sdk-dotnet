/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class PaymentReferences
    {
        /// <summary>
        /// Custom text provided by the merchant that is shown to the customer on
        /// their account statement, next to or instead of the registered merchant
        /// name. Support and maximum length depend on the payment method.
        /// <p />
        /// Note that there are restrictions on length and allowed characters that differ
        /// depending on the brand and scheme. If this string is too long it's usually
        /// truncated.
        /// </summary>
        public string DynamicDescriptor { get; set; }

        /// <summary>
        /// Custom text provided by the merchant that appears on their own settlement
        /// reports and on the reimbursement notices from the acquirer.
        /// <p />
        /// It allows the merchant to reconcile their payments in our report files
        /// and in their disputes.
        /// <p />
        /// It is advised to submit a unique value per transaction.<br />
        /// The value is returned in the baseTrxType/addlMercData element of the MRX file.
        /// </summary>
        public string MerchantReference { get; set; }
    }
}
