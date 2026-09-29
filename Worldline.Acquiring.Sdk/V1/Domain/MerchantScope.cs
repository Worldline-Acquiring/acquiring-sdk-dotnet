/*
 * This file was automatically generated.
 */
using Newtonsoft.Json;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class MerchantScope
    {
        /// <summary>
        /// Possible values are: BY_ACQUIRER_IDS, BY_MERCHANT_ROOT_IDS, BY_MERCHANT_IDS.
        /// </summary>
        [JsonProperty("merchantScopeType")]
        public virtual string MerchantScopeType { get; protected set; }
    }
}
