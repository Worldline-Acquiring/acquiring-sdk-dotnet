/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class CardPaymentDataForVerification
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
        /// Card data in plain text
        /// </summary>
        public PlainCardData CardData { get; set; }

        /// <summary>
        /// Card entry mode used in the transaction
        /// </summary>
        public string CardEntryMode { get; set; }

        /// <summary>
        /// Card data can be kept on file to support various use cases. It requires you to flag the transaction correctly.
        /// </summary>
        public CardOnFileData CardOnFileData { get; set; }

        /// <summary>
        /// Cardholder verification method used in the transaction
        /// </summary>
        public string CardholderVerificationMethod { get; set; }

        /// <summary>
        /// Request data for eCommerce transactions
        /// </summary>
        public ECommerceDataForAccountVerification EcommerceData { get; set; }

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
