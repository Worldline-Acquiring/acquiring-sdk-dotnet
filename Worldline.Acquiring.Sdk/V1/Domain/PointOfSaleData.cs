/*
 * This file was automatically generated.
 */
using System.Collections.Generic;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class PointOfSaleData
    {
        /// <summary>
        /// EMV data of the card as tag/value pairs.<br />
        /// It is needed when cardEntryMode is CHIP or CONTACTLESS.
        /// </summary>
        public IList<EmvDataItem> EmvData { get; set; }

        /// <summary>
        /// Encrypted data containing a PIN
        /// </summary>
        public string EncryptedPinBlock { get; set; }

        /// <summary>
        /// Indicate whether the request is made after a first one that resulted in a PIN request
        /// </summary>
        public bool? IsResponseToPinRequest { get; set; }

        /// <summary>
        /// Indicate whether the request is a retry with the same operation ID after a first request that resulted in a PIN request
        /// </summary>
        public bool? IsRetryWithTheSameOperationId { get; set; }

        /// <summary>
        /// Reference to the master key used to encrypt the PIN
        /// </summary>
        public string PinMasterKeyReference { get; set; }

        /// <summary>
        /// Track 2 data from the card<br />
        /// It is needed when cardEntryMode is MAGNETIC_STRIPE.
        /// </summary>
        public string Track2Data { get; set; }
    }
}
