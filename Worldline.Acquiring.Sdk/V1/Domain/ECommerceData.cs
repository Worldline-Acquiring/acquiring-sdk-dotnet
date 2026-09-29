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
        /// Strong customer authentication exemption request. Indicates the reason why the transaction may be exempt from SCA requirements.
        /// <p />
        /// Possible values are:
        /// <list type="bullet">
        ///   <item><description>LOW_VALUE_PAYMENT - Transaction amount is low enough to be exempt from SCA requirements.</description></item>
        ///   <item><description>SCA_DELEGATION - Strong Customer Authentication (SCA) has been performed through other means.</description></item>
        ///   <item><description>SECURE_CORPORATE_PAYMENT - The transaction is a secure corporate payment, using a corporate card.</description></item>
        ///   <item><description>TRANSACTION_RISK_ANALYSIS - The transaction risk has been analyzed and deemed low.</description></item>
        ///   <item><description>TRUSTED_BENEFICIARY - The beneficiary is a trusted entity with established relationship.</description></item>
        ///   <item><description>AUTHENTICATION_OUTAGE - The authentication service is currently unavailable.</description></item>
        /// </list>
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
