/*
 * This file was automatically generated.
 */
using System;
using System.Collections.Generic;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class ApiRefundResource
    {
        public CardPaymentDataForResource CardPaymentData { get; set; }

        /// <summary>
        /// Authorization approval code
        /// </summary>
        public string InitialAuthorizationCode { get; set; }

        public IList<SubOperationForRefund> Operations { get; set; }

        /// <summary>
        /// The identifier of the payment referenced by this refund.
        /// </summary>
        public string ReferencedPaymentId { get; set; }

        /// <summary>
        /// A set of references returned in responses
        /// </summary>
        public ApiReferencesForResponses References { get; set; }

        /// <summary>
        /// the ID of the refund
        /// </summary>
        public string RefundId { get; set; }

        /// <summary>
        /// The duration to wait after the initial submission before retrying the payment.<br />
        /// Expressed using ISO 8601 duration format, ex: PT2H for 2 hours.<br />
        /// This field is only present when the payment can be retried later.<br />
        /// PT0 means that the payment can be retried immediately.
        /// </summary>
        public string RetryAfter { get; set; }

        /// <summary>
        /// The status of the payment, refund or credit transfer
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Timestamp of the status in format yyyy-MM-ddTHH:mm:ssZ
        /// </summary>
        public DateTimeOffset StatusTimestamp { get; set; }

        /// <summary>
        /// Amount for the operation.
        /// </summary>
        public AmountData TotalAuthorizedAmount { get; set; }
    }
}
