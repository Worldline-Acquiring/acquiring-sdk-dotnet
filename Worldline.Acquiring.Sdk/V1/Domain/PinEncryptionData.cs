/*
 * This file was automatically generated.
 */
using Newtonsoft.Json;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class PinEncryptionData
    {
        /// <summary>
        /// Possible values are: AES_UKPT, DUKPT, ZPK.
        /// </summary>
        [JsonProperty("pinEncryptionType")]
        public virtual string PinEncryptionType { get; protected set; }
    }
}
