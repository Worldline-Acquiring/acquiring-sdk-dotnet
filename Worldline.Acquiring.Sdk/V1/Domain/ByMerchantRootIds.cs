/*
 * This file was automatically generated.
 */
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class ByMerchantRootIds : MerchantScope
    {
        [JsonProperty("merchantScopeType")]
        public override string MerchantScopeType => "BY_MERCHANT_ROOT_IDS";

        public IList<MerchantRootIdItem> MerchantRootIds { get; set; }
    }
}
