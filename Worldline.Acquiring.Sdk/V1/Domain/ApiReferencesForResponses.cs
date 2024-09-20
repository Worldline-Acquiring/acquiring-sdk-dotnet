/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class ApiReferencesForResponses
    {
        /// <summary>
        /// (PAR) Unique identifier associated with a specific cardholder PAN
        /// </summary>
        public string PaymentAccountReference { get; set; }

        /// <summary>
        /// Retrieval reference number for transaction, must be AN(12) if provided
        /// </summary>
        public string RetrievalReferenceNumber { get; set; }

        /// <summary>
        /// ID assigned by the scheme to identify a transaction through
        /// its whole lifecycle.
        /// </summary>
        public string SchemeTransactionId { get; set; }
    }
}
