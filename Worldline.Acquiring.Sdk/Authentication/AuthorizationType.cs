using System;

namespace Worldline.Acquiring.Sdk.Authentication
{
    public class AuthorizationType
    {
        #region enum
        public static readonly AuthorizationType OAuth2 = new AuthorizationType("OAuth2");
        #endregion

        #region Static
        /// <summary>
        /// Returns the enum value of the specified string.
        /// </summary>
        public static AuthorizationType GetValueOf(string aString)
        {
            if (aString?.Equals(OAuth2._stringValue) ?? false)
            {
                return OAuth2;
            }
            throw new ArgumentException("Unsupported Authorization");
        }
        #endregion

        public override string ToString()
        {
            return _stringValue;
        }

        private AuthorizationType(string stringValue)
        {
            _stringValue = stringValue;
        }

        private readonly string _stringValue;
    }
}
