/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DisputeMerchantDataBase
    {
        /// <summary>
        /// The unique identifier of the acquirer.
        /// </summary>
        public string AcquirerId { get; set; }

        /// <summary>
        /// The unique identifier of the merchant.
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// The root identifier of the merchant, which is the same for all sub-merchants under the same parent company.
        /// </summary>
        public string MerchantRootId { get; set; }
    }
}
