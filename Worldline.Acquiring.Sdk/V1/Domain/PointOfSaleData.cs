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
        /// Indicate whether the request is made after a first one that resulted in a PIN request
        /// </summary>
        public bool? IsResponseToPinRequest { get; set; }

        /// <summary>
        /// Indicate whether the request is a retry with the same operation ID after a first request that resulted in a PIN request
        /// </summary>
        public bool? IsRetryWithTheSameOperationId { get; set; }

        /// <summary>
        /// In case of online PIN verification, send this object with the appropriate values.
        /// <p />
        /// Depending on the acquirer, different PIN encryption types are supported. Please check with your
        /// Worldline contact which encryption type is supported for your account.
        /// </summary>
        public OnlinePinData OnlinePinData { get; set; }

        /// <summary>
        /// Track 2 data from the card<br />
        /// It is needed when cardEntryMode is MAGNETIC_STRIPE.
        /// </summary>
        public string Track2Data { get; set; }
    }
}
