/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class NetworkTokenData
    {
        /// <summary>
        /// Network token cryptogram
        /// </summary>
        public string Cryptogram { get; set; }

        /// <summary>
        /// Electronic Commerce Indicator<br />
        /// Value returned by the 3D Secure process that indicates the level of
        /// authentication.<br />
        /// Contains different values depending on the brand.
        /// </summary>
        public string Eci { get; set; }
    }
}
