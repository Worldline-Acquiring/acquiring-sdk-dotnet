/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class ECommerceDataForResponse
    {
        /// <summary>
        /// Result of Address Verification Result<br />
        /// Possible values:
        /// <list type="bullet">
        ///   <item><description>MATCH</description></item>
        ///   <item><description>ADDRESS_MATCH_POSTAL_CODE_MISMATCH</description></item>
        ///   <item><description>ADDRESS_MISMATCH_POSTAL_CODE_MATCH</description></item>
        ///   <item><description>ADDRESS_MATCH_POSTAL_CODE_NOT_VERIFIED</description></item>
        ///   <item><description>ADDRESS_NOT_VERIFIED_POSTAL_CODE_MATCH</description></item>
        ///   <item><description>MISMATCH</description></item>
        ///   <item><description>ERROR</description></item>
        ///   <item><description>NOT_VERIFIED</description></item>
        /// </list>
        /// </summary>
        public string AddressVerificationResult { get; set; }

        /// <summary>
        /// Result of card security code check<br />
        /// Possible values:
        /// <list type="bullet">
        ///   <item><description>MATCH</description></item>
        ///   <item><description>MISMATCH</description></item>
        ///   <item><description>NOT_VERIFIED</description></item>
        ///   <item><description>OMITTED</description></item>
        ///   <item><description>MISSING</description></item>
        /// </list>
        /// </summary>
        public string CardSecurityCodeResult { get; set; }
    }
}
