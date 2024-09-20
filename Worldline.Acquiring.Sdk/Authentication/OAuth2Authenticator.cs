using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.Json;

namespace Worldline.Acquiring.Sdk.Authentication
{
    /// <summary>
    /// <see cref="IAuthenticator"/> implementation using OAuth2.
    /// </summary>
    public class OAuth2Authenticator : IAuthenticator
    {
        /// <summary>
        /// Constructs a new OAuth2 authenticator.
        /// </summary>
        /// <param name="communicatorConfiguration">The configuration object containing the OAuth2 client id, client secret and token URI,
        ///                                         connect timeout, and socket timeout. None of these can be <c>null</c> or empty, and
        ///                                         the timeout values must be positive.</param>
        public OAuth2Authenticator(CommunicatorConfiguration communicatorConfiguration)
        {
            if (string.IsNullOrWhiteSpace(communicatorConfiguration.OAuth2ClientId))
            {
                throw new ArgumentException("clientId is required");
            }
            if (string.IsNullOrWhiteSpace(communicatorConfiguration.OAuth2ClientSecret))
            {
                throw new ArgumentException("clientSecret is required");
            }
            if (string.IsNullOrWhiteSpace(communicatorConfiguration.OAuth2TokenUri))
            {
                throw new ArgumentException("tokenUri is required");
            }
            if (communicatorConfiguration.SocketTimeout == null || communicatorConfiguration.SocketTimeout.Value.TotalMilliseconds <= 0)
            {
                throw new ArgumentException("socketTimeout is required and must be positive");
            }
            _clientId = communicatorConfiguration.OAuth2ClientId;
            _clientSecret = communicatorConfiguration.OAuth2ClientSecret;
            _oauth2TokenUri = new Uri(communicatorConfiguration.OAuth2TokenUri);
            _socketTimeout = communicatorConfiguration.SocketTimeout.Value;
            _proxy = communicatorConfiguration.Proxy;
        }

        public async Task<string> GetAuthorization(HttpMethod httpMethod, Uri resourceUri, IEnumerable<IRequestHeader> requestHeaders)
        {
            var tokenType = GetTokenType(resourceUri.AbsolutePath);
            await tokenType.Semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                if (IsAccessTokenNullOrExpired(tokenType.AccessToken))
                {
                    tokenType.AccessToken = await GetAccessToken(tokenType.Scopes).ConfigureAwait(false);
                }
                return "Bearer " + tokenType.AccessToken.AccessToken;
            }
            finally
            {
                tokenType.Semaphore.Release();
            }
        }

        private static bool IsAccessTokenNullOrExpired(OAuth2AccessToken accessToken)
        {
            return accessToken == null || accessToken.ExpirationTime < DateTime.Now;
        }

        private async Task<OAuth2AccessToken> GetAccessToken(string scopes)
        {
            using (var connection = new DefaultConnection(_socketTimeout, 1, _proxy))
            {
                var requestHeaders = new List<IRequestHeader>
                {
                    new EntityHeader("Content-Type", "application/x-www-form-urlencoded")
                };
                var requestBody = $"grant_type=client_credentials&client_id={_clientId}&client_secret={_clientSecret}&scope={scopes}";

                var startTime = DateTime.Now;

                return await connection.Post(_oauth2TokenUri, requestHeaders, requestBody, (status, body, headers) => {
                    var accessTokenResponse = DefaultMarshaller.Instance.Unmarshal<OAuth2AccessTokenResponse>(body);

                    if (status != HttpStatusCode.OK)
                    {
                        throw new OAuth2Exception($"There was an error while retrieving the OAuth2 access token: {accessTokenResponse.Error} - {accessTokenResponse.ErrorDescription}");
                    }

                    var accessTokenExpirationTime = startTime.AddSeconds(accessTokenResponse.ExpiresIn.Value);
                    return new OAuth2AccessToken(accessTokenResponse.AccessToken, accessTokenExpirationTime);
                }).ConfigureAwait(false);
            }
        }

        private TokenType GetTokenType(string fullPath)
        {
            foreach (var tokenTypeEntry in _accessTokens)
            {
                if (fullPath.EndsWith(tokenTypeEntry.Path) || fullPath.Contains(tokenTypeEntry.Path + "/"))
                {
                    return tokenTypeEntry;
                }
            }

            throw new OAuth2Exception("Scope could not be found for path " + fullPath);
        }

        private class TokenType
        {
            internal SemaphoreSlim Semaphore { get; } = new SemaphoreSlim(1);
            internal OAuth2AccessToken AccessToken { get; set; }
            internal string Path { get; }
            internal string Scopes { get; }

            public TokenType(string path, params string[] scopes)
            {
                Path = path;
                Scopes = string.Join(" ", scopes);
            }
        }

        // ReaderWriterLock and ReaderWriterLockSlim do not work with async/await.
        // Use a SemaphoreSlim instead. That does mean that multiple reads have to wait on each other,
        // but the read-only part is limited to checking the access token and its expiration timestamp,
        // which should take only a very short time
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly Uri _oauth2TokenUri;
        private readonly TimeSpan _socketTimeout;
        private readonly Proxy _proxy;

        // Only a limited amount of scopes may be sent in one request.
        // While at the moment all scopes fit in one request, keep this code so we can easily add more token types if necessary.
        // The empty path will ensure that all paths will match, as each full path ends with an empty string.
        private readonly List<TokenType> _accessTokens = new List<TokenType> {
            new TokenType("", "processing_payment", "processing_refund", "processing_credittransfer",
                "processing_accountverification", "processing_operation_reverse", "processing_dcc_rate", "services_ping")
        };
    }
}
