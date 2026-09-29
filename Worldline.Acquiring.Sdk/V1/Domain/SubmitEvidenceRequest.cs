/*
 * This file was automatically generated.
 */
using System.Collections.Generic;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class SubmitEvidenceRequest
    {
        public IList<DisputeDocumentIdItem> DocumentIds { get; set; }

        /// <summary>
        /// The long message text of the dispute entry, if applicable. This is typically used for communication entries
        /// to provide the content of the message sent by the acquirer to the merchant or vice versa.
        /// This field can contain a more detailed message than the <c>messageText</c> property.
        /// </summary>
        public string Elaboration { get; set; }

        /// <summary>
        /// If true, the response will include the full history of dispute entries related to the dispute.
        /// False by default.
        /// </summary>
        public bool? IncludeEntries { get; set; }

        /// <summary>
        /// Optional: Challenge only a partial amount of the dispute. By providing this value, you accept
        /// automatic liability for the remaining disputed amount.
        /// <p />
        /// Rules:
        /// <list type="bullet">
        ///   <item><description>Must be greater than 0</description></item>
        ///   <item><description>Must not exceed <c>originalDisputeAmount</c></description></item>
        ///   <item><description>All submitted evidence will be applied only to defending this partial amount</description></item>
        /// </list>
        /// <p />
        /// Example: If dispute is EUR 100 and <c>partialAmount</c> is EUR 30, you're defending EUR 30 and accepting
        /// liability for EUR 70.
        /// </summary>
        public AmountData PartialAmount { get; set; }

        /// <summary>
        /// The unique identifier of the user that triggered the dispute entry, if applicable.
        /// </summary>
        public string UserId { get; set; }
    }
}
