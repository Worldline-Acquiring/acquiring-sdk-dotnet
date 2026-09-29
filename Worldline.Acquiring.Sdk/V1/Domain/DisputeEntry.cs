/*
 * This file was automatically generated.
 */
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Worldline.Acquiring.Sdk.Json;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DisputeEntry
    {
        /// <summary>
        /// A list of documents related to the history entry.
        /// </summary>
        public IList<DisputeDocument> Documents { get; set; }

        /// <summary>
        /// The long message text of the dispute entry, if applicable. This is typically used for communication entries
        /// to provide the content of the message sent by the acquirer to the merchant or vice versa.
        /// This field can contain a more detailed message than the <c>messageText</c> property.
        /// </summary>
        public string Elaboration { get; set; }

        /// <summary>
        /// The category of the dispute entry.
        /// <p />
        /// Possible values are:
        /// <list type="bullet">
        ///   <item><description><c>DISPUTE</c>	(Transaction which drives the scheme dispute processing flow)</description></item>
        ///   <item><description><c>COMMUNICATION</c>	(Conversational and informational messages which are exchanged in the communication between Acquirer and Merchant)</description></item>
        ///   <item><description><c>EVIDENCE</c> (Merchant response to Acquirer Evidence Request (including liability acceptance))</description></item>
        ///   <item><description><c>POSTING</c> (Notification about upcoming Merchant account adjustments, executed by the Acquirer)</description></item>
        /// </list>
        /// </summary>
        public string EntryCategory { get; set; }

        /// <summary>
        /// The date and time when the dispute entry was made, in ISO 8601 format, but without the timezone designator.
        /// </summary>
        public string EntryDateTime { get; set; }

        /// <summary>
        /// The unique identifier for an dispute entry.
        /// </summary>
        public string EntryId { get; set; }

        /// <summary>
        /// The type of the dispute entry. The values for this field depend on the value of the <c>EntryCategory</c> field.
        /// <p />
        /// Possible values are:
        /// <list type="bullet">
        ///   <item><description>For DISPUTE entry Category:
        ///     <list type="bullet">
        ///       <item><description><c>Iss-Dsp</c> (Issuer dispute (full/partial))</description></item>
        ///       <item><description><c>Iss-DspRev</c> (Issuer reversed dispute)</description></item>
        ///       <item><description><c>Iss-ArbDsp</c> (Issuer Arbitration dispute (full/partial))</description></item>
        ///       <item><description><c>Iss-PArb</c> (Issuer Pre-Arbitration (full/partial))</description></item>
        ///       <item><description><c>Iss-PComp</c> (Issuer Pre-Compliance)</description></item>
        ///       <item><description><c>Iss-Arb</c> (Issuer Arbitration)</description></item>
        ///       <item><description>'Iss-Comp' (Issuer Compliance)</description></item>
        ///       <item><description><c>Acq-PArb</c> (Acquirer Pre-Arbitration (full/partial))</description></item>
        ///       <item><description><c>Acq-PComp</c> (Acquirer Pre-Compliance)</description></item>
        ///       <item><description><c>Acq-Arb</c> (Acquirer Arbitration)</description></item>
        ///       <item><description><c>Acq-Comp</c> (Acquirer Compliance)</description></item>
        ///       <item><description><c>Acq-DspDecline</c> (Acquirer Declines Dispute (full/partial))</description></item>
        ///       <item><description><c>Acq-ArbDspDecline</c> (Acquirer Declines Arbitration Dispute (full/partial))</description></item>
        ///       <item><description><c>Acq-PArbDecline</c> (Acquirer Declines Pre-Arbitration (full/partial))</description></item>
        ///       <item><description><c>Acq-PCompDecline</c> (Acquirer Declines Pre-Compliance (full/partial))</description></item>
        ///       <item><description><c>Iss-PArbDecline</c> (Issuer Declines Pre-Arbitration (full/partial))</description></item>
        ///       <item><description><c>Iss-PCompDecline</c> (Issuer Declines Pre-Compliance (full/partial))</description></item>
        ///       <item><description><c>Acq-MchLost</c> (Dispute case is lost, Liability on Acquirer)</description></item>
        ///       <item><description><c>Acq-MchWon</c> (Dispute case is won, Liability on Issuer)</description></item>
        ///     </list>
        ///   </description></item>
        ///   <item><description>For COMMUNICATION entryCategory:
        ///     <list type="bullet">
        ///       <item><description><c>Acq-MsgToMch</c> (Acquirer sent a message to the Merchant)</description></item>
        ///       <item><description><c>Mch-MsgToAcq</c> (Merchant sent a message to the Acquirer)</description></item>
        ///     </list>
        ///   </description></item>
        ///   <item><description>For EVIDENCE entryCategory:
        ///     <list type="bullet">
        ///       <item><description><c>Acq-EvidenceReq</c> (Acquirer request evidence from Merchant)</description></item>
        ///       <item><description><c>Acq-EvidenceRej</c> (Acquirer reject the evidence submitted by the Merchant)</description></item>
        ///       <item><description><c>Mch-Evidence</c> (Merchant submitted evidence)</description></item>
        ///       <item><description><c>Mch-Accept</c> (Merchant accepted liability)</description></item>
        ///     </list>
        ///   </description></item>
        ///   <item><description>For POSTING entryCategory:
        ///     <list type="bullet">
        ///       <item><description><c>Acq-CreditAdj</c> (Acquirer credited the Merchant)</description></item>
        ///       <item><description><c>Acq-DebitAdj</c> (Acquirer debited the Merchant)</description></item>
        ///     </list>
        ///   </description></item>
        /// </list>
        /// </summary>
        public string EntryType { get; set; }

        /// <summary>
        /// The human readable description of the <c>EntryType</c> field.
        /// </summary>
        public string EntryTypeDescription { get; set; }

        /// <summary>
        /// The message text of the dispute entry, if applicable. This is typically used for communication entries
        /// to provide the content of the message sent by the acquirer to the merchant or vice versa.
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// The questionnaire provided by the card scheme for the dispute, if applicable. This is typically used for communication entries
        /// to provide the content of the questionnaire sent by the acquirer to the merchant in case the card scheme requires a specific
        /// set of questions to be answered by the merchant in order to provide evidence for the dispute case.
        /// </summary>
        public string Questionnaire { get; set; }

        /// <summary>
        /// The date when a response to the dispute is due.
        /// This field is typically relevant when the <c>disputeStatus</c> is &quot;EVIDENCE_REQUESTED&quot;, to indicate the deadline to respond
        /// to the dispute.
        /// </summary>
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? ResponseDueDate { get; set; }

        /// <summary>
        /// The reason provided by the card scheme for the dispute.
        /// </summary>
        public string SchemeReason { get; set; }

        /// <summary>
        /// The human readable description of the reason provided by the card scheme for the dispute.
        /// </summary>
        public string SchemeReasonDescription { get; set; }

        /// <summary>
        /// Amount for the operation.
        /// </summary>
        public AmountData SettlementAmount { get; set; }

        /// <summary>
        /// Amount for the operation.
        /// </summary>
        public AmountData TransactionAmount { get; set; }

        /// <summary>
        /// The unique identifier of the user that triggered the dispute entry, if applicable.
        /// </summary>
        public string UserId { get; set; }
    }
}
