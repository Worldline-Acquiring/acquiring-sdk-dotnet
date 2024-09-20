/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class MerchantData
    {
        /// <summary>
        /// Street address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Address city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Address country code, ISO 3166 international standard
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Merchant category code (MCC)
        /// </summary>
        public int? MerchantCategoryCode { get; set; }

        /// <summary>
        /// Merchant name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Address postal code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Address state code, only supplied if country is US or CA
        /// </summary>
        public string StateCode { get; set; }
    }
}
