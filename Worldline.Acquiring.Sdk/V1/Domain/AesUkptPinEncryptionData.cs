/*
 * This file was automatically generated.
 */
using Newtonsoft.Json;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class AesUkptPinEncryptionData : PinEncryptionData
    {
        [JsonProperty("pinEncryptionType")]
        public override string PinEncryptionType => "AES_UKPT";

        /// <summary>
        /// Generation/Version of the master key that was agreed with the partner
        /// </summary>
        public int? KeyGeneration { get; set; }

        /// <summary>
        /// 16-byte binary random value to derive the PIN encryption session key
        /// </summary>
        public string RandomValue { get; set; }
    }
}
