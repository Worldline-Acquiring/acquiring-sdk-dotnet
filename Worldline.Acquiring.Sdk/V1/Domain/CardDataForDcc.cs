/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class CardDataForDcc
    {
        /// <summary>
        /// Used to determine the currency of the card.
        /// The first 12 digits of the card number.
        /// The BIN number is on the first 6 or 8 digits.
        /// Some issuers are using subranges for different countries on digits
        /// 9-12.
        /// </summary>
        public string Bin { get; set; }

        /// <summary>
        /// The card brand
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// The country code of the card
        /// </summary>
        public string CardCountryCode { get; set; }
    }
}
