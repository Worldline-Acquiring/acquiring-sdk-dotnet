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
        /// Customer Service Data
        /// </summary>
        public CustomerServiceData CustomerServiceData { get; set; }

        /// <summary>
        /// Merchant category code (MCC)
        /// </summary>
        public int? MerchantCategoryCode { get; set; }

        /// <summary>
        /// Merchant name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Payment Facilitator identifier as assigned by Worldline
        /// </summary>
        public string PaymentFacilitatorId { get; set; }

        /// <summary>
        /// Address postal code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Address state code, only supplied if country is US or CA
        /// </summary>
        public string StateCode { get; set; }

        /// <summary>
        /// Sub-merchant identifier in the context of a Payment Facilitator.
        /// </summary>
        public string SubMerchantId { get; set; }

        /// <summary>
        /// Applicable for Payment Facilitator submerchants located in France, Belgium or Luxembourg &amp;
        /// having a valid national SIRET/Tax ID when using Bambora as the acquirer.
        /// </summary>
        public string TaxId { get; set; }
    }
}
