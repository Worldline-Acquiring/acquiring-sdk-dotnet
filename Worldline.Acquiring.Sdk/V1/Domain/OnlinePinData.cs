/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class OnlinePinData
    {
        /// <summary>
        /// Encrypted data containing a PIN
        /// </summary>
        public string EncryptedPinBlock { get; set; }

        /// <summary>
        /// ISO 9564 based PIN block format.
        /// <p />
        /// Worldline acquiring only supports the following format:
        /// <list type="bullet">
        ///   <item><description>4 - ISO-4</description></item>
        /// </list>
        /// <p />
        /// Bambora acquiring supports the following formats:
        /// <list type="bullet">
        ///   <item><description>0 - ISO 9564-1 Format 0 (Standard PIN block format with PAN XOR, commonly used with 3DES / DUKPT).</description></item>
        ///   <item><description>1 - ISO 9564-1 Format 1 (PIN block with transaction sequence/random number).</description></item>
        ///   <item><description>2 - ISO 9564-1 Format 2 (Primarily used for offline/smart cards).</description></item>
        ///   <item><description>3 - ISO 9564-1 Format 3 (Similar to Format 0 with random fill digits).</description></item>
        ///   <item><description>4 - ISO 9564-1 Format 4 (AES-256 encrypted PIN block format, required for modern Online PIN / Tap on Mobile CVM solutions).</description></item>
        /// </list>
        /// </summary>
        public int? PinBlockFormat { get; set; }

        /// <summary>
        /// PIN encryption details used for the <c>encryptedPinBlock</c>.
        /// <p />
        /// The following variants are supported:
        /// <list type="bullet">
        ///   <item><description>AES_UKPT - Used in combination with Worldline acquirers</description></item>
        ///   <item><description>DUKPT - Used in combination with the Bambora acquirer</description></item>
        ///   <item><description>ZPK - Zone PIN Key, used in combination with the Bambora acquirer</description></item>
        /// </list>
        /// </summary>
        public PinEncryptionData PinEncryptionData { get; set; }
    }
}
