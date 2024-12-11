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
        /// Cardholder verification method used in the transaction
        /// </summary>
        public string CardholderVerificationMethod { get; set; }

        public NetworkTokenData NetworkTokenData { get; set; }

        /// <summary>
        /// Type of wallet, values are assigned by card schemes, e.g. 101
        /// for MasterPass in eCommerce, 102 for MasterPass NFC, 103 for Apple Pay,
        /// 216 for Google Pay and 217 for Samsung Pay
        /// </summary>
        public string WalletId { get; set; }
    }
}
