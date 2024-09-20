using Newtonsoft.Json;

namespace Worldline.Acquiring.Sdk.Authentication
{
    internal class OAuth2AccessTokenResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        internal string AccessToken { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        internal long? ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "error")]
        internal string Error { get; set; }

        [JsonProperty(PropertyName = "error_description")]
        internal string ErrorDescription { get; set; }
    }
}
