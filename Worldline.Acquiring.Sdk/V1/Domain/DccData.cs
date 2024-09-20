/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DccData
    {
        /// <summary>
        /// Amount of transaction formatted according to card scheme
        /// specifications.
        /// E.g. 100 for 1.00 EUR. Either this or amount must be present.
        /// </summary>
        public long? Amount { get; set; }

        /// <summary>
        /// Currency conversion rate in decimal notation.<br />
        /// Either this or isoConversionRate must be present
        /// </summary>
        public decimal? ConversionRate { get; set; }

        /// <summary>
        /// Alpha-numeric ISO 4217 currency code for transaction, e.g. EUR
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Number of decimals in the amount
        /// </summary>
        public int? NumberOfDecimals { get; set; }
    }
}
