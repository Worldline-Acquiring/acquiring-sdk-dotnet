/*
 * This file was automatically generated.
 */
using Newtonsoft.Json;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class ZpkPinEncryptionData : PinEncryptionData
    {
        [JsonProperty("pinEncryptionType")]
        public override string PinEncryptionType => "ZPK";

        /// <summary>
        /// ID of the Zone PIN Key if ZPK encryption is used for the PIN block
        /// </summary>
        public string ZonePinKeyId { get; set; }
    }
}
