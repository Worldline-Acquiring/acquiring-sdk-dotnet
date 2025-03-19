/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class CardOnFileData
    {
        /// <summary>
        /// When card data is stored you need to flag its purpose using <c>transactionType</c> and the intended <c>futureUse</c> of the card data.
        /// </summary>
        public InitialCardOnFileData InitialCardOnFileData { get; set; }

        /// <summary>
        /// Indicate whether this is the initial Card on File transaction or not
        /// </summary>
        public bool? IsInitialTransaction { get; set; }

        /// <summary>
        /// When you are using stored card you need to again specify the <c>transactionType</c>. All values are supported when the MERCHANT is
        /// the initiator of the transaction. When the CARDHOLDER is the initiator of the transaction, only <c>UNSCHEDULED_CARD_ON_FILE</c> is
        /// supported. For all cases when the MERCHANT is the initiator of the transaction, the <c>initialSchemeTransactionId</c> property
        /// is mandatory.
        /// </summary>
        public SubsequentCardOnFileData SubsequentCardOnFileData { get; set; }
    }
}
