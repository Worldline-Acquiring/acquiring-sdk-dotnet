/*
 * This file was automatically generated.
 */
using Newtonsoft.Json;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DukptPinEncryptionData : PinEncryptionData
    {
        [JsonProperty("pinEncryptionType")]
        public override string PinEncryptionType => "DUKPT";

        /// <summary>
        /// Key Serial Number (KSN) if DUKPT encryption is used for the PIN block (3DES: 10 b, AES: 12 b)
        /// </summary>
        public string KeySerialNumber { get; set; }
    }
}
