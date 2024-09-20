/*
 * This file was automatically generated.
 */
using System;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class RateData
    {
        /// <summary>
        /// The exchange rate
        /// </summary>
        public decimal? ExchangeRate { get; set; }

        /// <summary>
        /// The inverted exchange rate
        /// </summary>
        public decimal? InvertedExchangeRate { get; set; }

        /// <summary>
        /// The mark up applied on the rate (in percentage).
        /// </summary>
        public decimal? MarkUp { get; set; }

        /// <summary>
        /// The source of the rate the markup is based upon.
        /// If the cardholder and the merchant are based in Europe, the
        /// mark up is calculated based on the
        /// rates provided by the European Central Bank.
        /// </summary>
        public string MarkUpBasis { get; set; }

        /// <summary>
        /// The date and time of the quotation
        /// </summary>
        public DateTimeOffset QuotationDateTime { get; set; }
    }
}
