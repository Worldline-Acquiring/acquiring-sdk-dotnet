/*
 * This file was automatically generated.
 */
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class ByMerchantIds : MerchantScope
    {
        [JsonProperty("merchantScopeType")]
        public override string MerchantScopeType => "BY_MERCHANT_IDS";

        public IList<MerchantIdItem> MerchantIds { get; set; }
    }
}
