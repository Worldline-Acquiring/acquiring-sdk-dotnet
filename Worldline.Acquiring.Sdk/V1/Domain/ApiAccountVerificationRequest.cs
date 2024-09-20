/*
 * This file was automatically generated.
 */
using System;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class ApiAccountVerificationRequest
    {
        /// <summary>
        /// Card data
        /// </summary>
        public CardPaymentDataForVerification CardPaymentData { get; set; }

        /// <summary>
        /// Merchant Data
        /// </summary>
        public MerchantData Merchant { get; set; }

        /// <summary>
        /// A globally unique identifier of the operation, generated by you.<br />
        /// We advise you to submit a UUID or an identifier composed of an arbitrary string
        /// and a UUID/URL-safe Base64 UUID (RFC 4648 §5).<br />
        /// It's used to detect duplicate requests or to reference an operation in
        /// technical reversals.
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// Payment References
        /// </summary>
        public PaymentReferences References { get; set; }

        /// <summary>
        /// Timestamp of transaction in ISO 8601 format (YYYY-MM-DDThh:mm:ss+TZD)<br />
        /// It can be expressed in merchant time zone (ex: 2023-10-10T08:00+02:00)
        /// or in UTC (ex: 2023-10-10T08:00Z)
        /// </summary>
        public DateTimeOffset TransactionTimestamp { get; set; }
    }
}
