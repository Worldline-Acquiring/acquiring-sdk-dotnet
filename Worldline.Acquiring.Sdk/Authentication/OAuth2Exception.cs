using System;

namespace Worldline.Acquiring.Sdk.Authentication
{
    /// <summary>
    /// Indicates an exception regarding the authorization with the Worldline OAuth2 Authorization Server.
    /// </summary>
    public class OAuth2Exception : Exception
    {
        public OAuth2Exception(string message) : base(message)
        {
        }
    }
}
