/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class SignedAmountData
    {
        /// <summary>
        /// Amount of transaction formatted according to card scheme
        /// specifications.
        /// E.g. 100 for 1.00 EUR.
        /// </summary>
        public long? Amount { get; set; }

        /// <summary>
        /// Alpha-numeric ISO 4217 currency code for transaction, e.g. EUR
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Indicates whether the dispute is for a debit or credit transaction.<br />
        /// Possible values are:
        /// <list type="bullet">
        ///   <item><description>DEBIT (The merchant receives funds)</description></item>
        ///   <item><description>CREDIT (The merchant loses funds)</description></item>
        /// </list>
        /// </summary>
        public string DebitCreditIndicator { get; set; }

        /// <summary>
        /// Number of decimals in the amount
        /// </summary>
        public int? NumberOfDecimals { get; set; }
    }
}
