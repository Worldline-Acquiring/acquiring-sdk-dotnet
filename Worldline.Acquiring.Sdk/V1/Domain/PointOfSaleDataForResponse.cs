/*
 * This file was automatically generated.
 */
using System.Collections.Generic;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class PointOfSaleDataForResponse
    {
        /// <summary>
        /// EMV data of the card as tag/value pairs.
        /// </summary>
        public IList<EmvDataItem> EmvData { get; set; }

        /// <summary>
        /// Last 4 digits of the PAN
        /// </summary>
        public string PanLast4Digits { get; set; }

        /// <summary>
        /// Number of PIN retries
        /// </summary>
        public int? PinRetryCounter { get; set; }
    }
}
