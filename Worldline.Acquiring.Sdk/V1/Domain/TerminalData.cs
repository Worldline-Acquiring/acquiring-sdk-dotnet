/*
 * This file was automatically generated.
 */
using System.Collections.Generic;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class TerminalData
    {
        /// <summary>
        /// Indicate whether the terminal allow one single tap for a contactless transaction
        /// followed by a PIN entry if the contactless transaction is refused
        /// </summary>
        public bool? AllowSingleTap { get; set; }

        /// <summary>
        /// Card reading capabilities of the terminal.
        /// </summary>
        public IList<string> CardReadingCapabilities { get; set; }

        /// <summary>
        /// Level of security for a terminal activated with the use of a card (Cardholder Activated Terminal)
        /// </summary>
        public string CardholderActivatedTerminalLevel { get; set; }

        /// <summary>
        /// Indicate whether the terminal is attended or not
        /// </summary>
        public bool? IsAttendedTerminal { get; set; }

        /// <summary>
        /// The capabilities of the terminal to enter the PIN
        /// <list type="bullet">
        ///   <item><description>UNKNOWN : Unspecified or unknown</description></item>
        ///   <item><description>PRESENT : Terminal has PIN entry capability</description></item>
        ///   <item><description>ABSENT : Terminal does not have PIN entry capability</description></item>
        ///   <item><description>MPOS_SOFTWARE_BASED_PIN : Mobile POS with PIN entry capability</description></item>
        ///   <item><description>NOT_OPERATIVE : Terminal has PIN entry capability but PIN pad is not currently operative</description></item>
        /// </list>
        /// </summary>
        public string PinEntryCapability { get; set; }

        /// <summary>
        /// The identifier of the terminal
        /// </summary>
        public string TerminalId { get; set; }

        /// <summary>
        /// The location of the terminal
        /// </summary>
        public string TerminalLocation { get; set; }
    }
}
