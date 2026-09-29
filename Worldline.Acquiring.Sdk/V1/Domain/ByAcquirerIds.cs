/*
 * This file was automatically generated.
 */
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class ByAcquirerIds : MerchantScope
    {
        [JsonProperty("merchantScopeType")]
        public override string MerchantScopeType => "BY_ACQUIRER_IDS";

        public IList<string> AcquirerIds { get; set; }
    }
}
