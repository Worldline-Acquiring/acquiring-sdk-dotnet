/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DisputeMerchantData : DisputeMerchantDataBase
    {
        /// <summary>
        /// Merchant category code (MCC)
        /// </summary>
        public int? MerchantCategoryCode { get; set; }

        /// <summary>
        /// The city where the merchant is located.
        /// </summary>
        public string MerchantCity { get; set; }

        /// <summary>
        /// The country code of the merchant's location in ISO 3166-1 alpha-2 format.
        /// </summary>
        public string MerchantCountryCode { get; set; }

        /// <summary>
        /// Merchant name
        /// </summary>
        public string MerchantName { get; set; }
    }
}
