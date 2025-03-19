/*
 * This file was automatically generated.
 */
using System;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class ApiPaymentSummaryForResponse
    {
        /// <summary>
        /// the ID of the payment
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// A set of references returned in responses
        /// </summary>
        public ApiReferencesForResponses References { get; set; }

        /// <summary>
        /// The status of the payment, refund or credit transfer<br />
        /// Possible values are:
        /// <list type="bullet">
        ///   <item><description>AUTHORIZED</description></item>
        ///   <item><description>NOT_AUTHORIZED</description></item>
        ///   <item><description>PENDING</description></item>
        ///   <item><description>PENDING_CAPTURE</description></item>
        ///   <item><description>CONFIRMED</description></item>
        ///   <item><description>REVERSED</description></item>
        ///   <item><description>CANCELLED</description></item>
        /// </list>
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Timestamp of the status in format yyyy-MM-ddTHH:mm:ssZ
        /// </summary>
        public DateTimeOffset StatusTimestamp { get; set; }
    }
}
