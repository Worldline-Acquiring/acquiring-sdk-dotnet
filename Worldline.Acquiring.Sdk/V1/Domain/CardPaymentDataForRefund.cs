/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class CardPaymentDataForRefund
    {
        /// <summary>
        /// The card brand
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// The party responsible for the brand selection.
        /// </summary>
        public string BrandSelector { get; set; }

        /// <summary>
        /// If true the transaction will be authorized and captured immediately
        /// </summary>
        public bool? CaptureImmediately { get; set; }

        /// <summary>
        /// Card data in plain text
        /// </summary>
        public PlainCardData CardData { get; set; }

        /// <summary>
        /// Card entry mode used in the transaction
        /// </summary>
        public string CardEntryMode { get; set; }

        /// <summary>
        /// Method used by the terminal or the e-commerce website to verify that the customer is the legitimate cardholder (a.k.a. CVM)
        /// <list type="bullet">
        ///   <item><description>NONE: no cardholder verification performed</description></item>
        ///   <item><description>CARD_SECURITY_CODE: the customer provided the card verification value (3 or 4 digits)</description></item>
        ///   <item><description>THREE_DS: the customer completed an additional verification step with the card issuer</description></item>
        ///   <item><description>SIGNATURE: the terminal prompted for a signature</description></item>
        ///   <item><description>ONLINE_PIN: the terminal verifies the PIN online with the card issuer</description></item>
        ///   <item><description>OFFLINE_PIN: the terminal verified the PIN with the EMV chip on the card</description></item>
        ///   <item><description>CARDHOLDER_DEVICE: the cardholder device prompted the cardholder for authentication (a.k.a. CDCVM). Mainly used in transactions where digital wallets are involved</description></item>
        /// </list>
        /// </summary>
        public string CardholderVerificationMethod { get; set; }

        public NetworkTokenData NetworkTokenData { get; set; }

        /// <summary>
        /// Request data for Point Of Sale (POS) or &quot;in person&quot; Transaction
        /// </summary>
        public PointOfSaleData PointOfSaleData { get; set; }

        /// <summary>
        /// Type of wallet, values are assigned by card schemes, e.g.
        /// <list type="bullet">
        ///   <item><description>101 for MasterPass in eCommerce</description></item>
        ///   <item><description>102 for MasterPass NFC</description></item>
        ///   <item><description>103 for Apple Pay</description></item>
        ///   <item><description>216 for Google Pay</description></item>
        ///   <item><description>217 for Samsung Pay</description></item>
        ///   <item><description>327 to indicate the usage of Network tokens in the transaction</description></item>
        /// </list>
        /// </summary>
        public string WalletId { get; set; }
    }
}
