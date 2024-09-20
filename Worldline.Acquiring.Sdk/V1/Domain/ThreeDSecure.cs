/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class ThreeDSecure
    {
        /// <summary>
        /// MasterCard AAV in original base64 encoding or Visa, DinersClub,
        /// UnionPay or JCB CAVV in either hexadecimal or base64 encoding
        /// </summary>
        public string AuthenticationValue { get; set; }

        /// <summary>
        /// 3D Secure 2.x directory server transaction ID
        /// </summary>
        public string DirectoryServerTransactionId { get; set; }

        /// <summary>
        /// Electronic Commerce Indicator<br />
        /// Value returned by the 3D Secure process that indicates the level of
        /// authentication.<br />
        /// Contains different values depending on the brand.
        /// </summary>
        public string Eci { get; set; }

        /// <summary>
        /// 3D Secure type used in the transaction
        /// </summary>
        public string ThreeDSecureType { get; set; }

        /// <summary>
        /// 3D Secure version
        /// </summary>
        public string Version { get; set; }
    }
}
