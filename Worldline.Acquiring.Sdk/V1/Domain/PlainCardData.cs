/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class PlainCardData
    {
        /// <summary>
        /// Card number (PAN, network token or DPAN).
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// The security code indicated on the card<br />
        /// Based on the card brand, it can be 3 or 4 digits long<br />
        /// and have different names: CVV2, CVC2, CVN2, CID, CVC, CAV2, etc.
        /// </summary>
        public string CardSecurityCode { get; set; }

        /// <summary>
        /// Card or token expiry date in format MMYYYY
        /// </summary>
        public string ExpiryDate { get; set; }
    }
}
