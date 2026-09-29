/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class CustomerServiceData
    {
        /// <summary>
        /// Submerchant's customer service email address.
        /// Applicable only for Amex transactions but this field could be set for other
        /// scheme transactions too.
        /// Mandatory for all card not present transactions processed through Bambora.
        /// </summary>
        public string CustomerServiceEmail { get; set; }

        /// <summary>
        /// Submerchant's customer service phone number that can be used for transaction inquiries.
        /// Applicable for MasterCard transactions but this field could be set for other
        /// scheme transactions too.
        /// Optional for all card not present transactions. Either <c>CustomerServiceUrl</c> or
        /// <c>CustomerServicePhoneNumber</c> is mandatory for card present transactions.
        /// </summary>
        public string CustomerServicePhoneNumber { get; set; }

        /// <summary>
        /// Submerchant's customer service portal URL
        /// Applicable for MasterCard transactions but this field could be set for other
        /// scheme transactions too.
        /// Mandatory for all card not present transactions processed through Bambora.
        /// Either <c>CustomerServiceUrl</c> or <c>CustomerServicePhoneNumber</c> is mandatory for card
        /// present transactions.
        /// </summary>
        public string CustomerServiceUrl { get; set; }
    }
}
