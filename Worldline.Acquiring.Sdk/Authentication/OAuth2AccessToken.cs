using System;

namespace Worldline.Acquiring.Sdk.Authentication
{
    internal class OAuth2AccessToken
    {
        internal OAuth2AccessToken(string accessToken, DateTime expirationTime)
        {
            AccessToken = accessToken;
            ExpirationTime = expirationTime;
        }

        internal string AccessToken { get; }

        internal DateTime ExpirationTime { get; }
    }
}
