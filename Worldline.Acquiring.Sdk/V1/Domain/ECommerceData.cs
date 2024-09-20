/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class ECommerceData
    {
        /// <summary>
        /// Address Verification System data
        /// </summary>
        public AddressVerificationData AddressVerificationData { get; set; }

        /// <summary>
        /// Strong customer authentication exemption request
        /// </summary>
        public string ScaExemptionRequest { get; set; }

        /// <summary>
        /// 3D Secure data.<br />
        /// Please note that if AAV or CAVV or equivalent is
        /// missing, transaction should not be flagged as 3D Secure.
        /// </summary>
        public ThreeDSecure ThreeDSecure { get; set; }
    }
}
