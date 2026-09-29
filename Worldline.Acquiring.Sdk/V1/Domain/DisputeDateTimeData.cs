/*
 * This file was automatically generated.
 */
using System;
using Newtonsoft.Json;
using Worldline.Acquiring.Sdk.Json;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DisputeDateTimeData
    {
        /// <summary>
        /// The date and time when the dispute was closed, in ISO 8601 format, but without the timezone designator.
        /// Only present if the dispute is closed.
        /// </summary>
        public string ClosedDateTime { get; set; }

        /// <summary>
        /// The date and time when the status of the dispute was last updated, in ISO 8601 format, but without the timezone designator.
        /// When the dispute case is first created the value will be equal to the <c>OpenedDateTime</c> property.
        /// As dispute process continues, this value changes.
        /// </summary>
        public string LastStatusChangedDateTime { get; set; }

        /// <summary>
        /// The date and time when the dispute was opened, in ISO 8601 format, but without the timezone designator.
        /// </summary>
        public string OpenedDateTime { get; set; }

        /// <summary>
        /// The date when a response to the dispute is due.
        /// This field is typically relevant when the <c>disputeStatus</c> is &quot;EVIDENCE_REQUESTED&quot;, to indicate the deadline to respond
        /// to the dispute.
        /// </summary>
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? ResponseDueDate { get; set; }
    }
}
