/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class AcceptDisputeLiabilityRequest
    {
        /// <summary>
        /// If true, the response will include the full history of dispute entries related to the dispute.
        /// False by default.
        /// </summary>
        public bool? IncludeEntries { get; set; }

        /// <summary>
        /// The message text of the dispute entry, if applicable. This is typically used for communication entries
        /// to provide the content of the message sent by the acquirer to the merchant or vice versa.
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// The unique identifier of the user that triggered the dispute entry, if applicable.
        /// </summary>
        public string UserId { get; set; }
    }
}
