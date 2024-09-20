using System;
using System.Collections.Generic;
using System.Net.Http;
using Worldline.Acquiring.Sdk.Authentication;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.Domain;

namespace Worldline.Acquiring.Sdk
{
    /// <summary>
    /// Configuration for the communicator.
    /// </summary>
    public class CommunicatorConfiguration
    {
        /// <summary>
        /// The default number of maximum connections
        /// </summary>
        public const int DefaultMaxConnections = 10;

        /// <summary>
        /// Gets or sets the Worldline Acquiring platform API endpoint URI.
        /// </summary>
        public Uri ApiEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the connect timeout
        /// </summary>
        public TimeSpan? ConnectTimeout { get; set; }

        /// <summary>
        /// Gets or sets the socket timeout
        /// </summary>
        public TimeSpan? SocketTimeout { get; set; }

        /// <summary>
        /// Gets or sets the maximal number of connections
        /// </summary>
        public int MaxConnections { get; set; } = DefaultMaxConnections;

        /// <summary>
        /// Gets or sets the type of the authorization.
        /// </summary>
        public AuthorizationType AuthorizationType { get; set; } = AuthorizationType.OAuth2;

        /// <summary>
        /// Gets or sets an id used for authorization. The meaning of this id is different for each authorization type.
        /// For instance, for <see cref="AuthorizationType.OAuth2"/> this is the client id.
        /// </summary>
        public string AuthorizationId { get; set; }

        /// <summary>
        /// Gets or sets a secret used for authorization. The meaning of this secret is different for each authorization type.
        /// For instance, for <see cref="AuthorizationType.OAuth2"/> this is the client secret.
        /// </summary>
        public string AuthorizationSecret { get; set; }

        /// <summary>
        /// Gets or sets the OAuth2 client id.
        /// <p>This property is an alias for <see cref="AuthorizationId"/>.</p>
        /// </summary>
        public string OAuth2ClientId
        {
            get => AuthorizationId;
            set => AuthorizationId = value;
        }

        /// <summary>
        /// Gets or sets the OAuth2 client secret.
        /// <p>This property is an alias for <see cref="AuthorizationSecret"/>.</p>
        /// </summary>
        public string OAuth2ClientSecret
        {
            get => AuthorizationSecret;
            set => AuthorizationSecret = value;
        }

        /// <summary>
        /// Gets or sets the OAuth2 token URI.
        /// </summary>
        public string OAuth2TokenUri { get; set; }

        /// <summary>
        /// Gets the proxy object
        /// </summary>
        public Proxy Proxy => ProxyUri != null ? new Proxy { Username = ProxyUserName, Password = ProxyPassword, Uri = ProxyUri } : null;

        /// <summary>
        /// Gets or sets the proxy URI.
        /// </summary>
        public Uri ProxyUri { get; set; }

        /// <summary>
        /// Gets or sets the proxy username.
        /// </summary>
        public string ProxyUserName { get; set; }

        /// <summary>
        /// Gets or sets the proxy password.
        /// </summary>
        public string ProxyPassword { get; set; }

        /// <summary>
        /// Gets or sets the integrator.
        /// </summary>
        public string Integrator { get; set; }

        /// <summary>
        /// Gets or sets the shoppingcart extension.
        /// </summary>
        public ShoppingCartExtension ShoppingCartExtension { get; set; }

        /// <summary>
        /// Gets or sets a custom HttpClientHandler to be used by <see cref="DefaultConnection"/>.
        /// </summary>
        public HttpClientHandler HttpClientHandler { get; set; }

        public CommunicatorConfiguration()
        {

        }

        public CommunicatorConfiguration(IDictionary<string, string> properties)
        {
            if (properties != null)
            {
                ApiEndpoint = GetApiEndpoint(properties);
                AuthorizationType = AuthorizationType.GetValueOf(GetProperty(properties, "acquiring.api.authorizationType"));
                OAuth2TokenUri = GetProperty(properties, "acquiring.api.oauth2.tokenUri");

                var connectTimeout = int.Parse(GetProperty(properties, "acquiring.api.connectTimeout"));
                ConnectTimeout = connectTimeout >= 0 ? (TimeSpan?)TimeSpan.FromMilliseconds(connectTimeout) : null;

                var socketTimeout = int.Parse(GetProperty(properties, "acquiring.api.socketTimeout"));
                SocketTimeout = socketTimeout >= 0 ? (TimeSpan?)TimeSpan.FromMilliseconds(socketTimeout) : null;

                MaxConnections = GetProperty(properties, "acquiring.api.maxConnections", DefaultMaxConnections);

                var proxyUri = GetProperty(properties, "acquiring.api.proxy.uri");
                var proxyUser = GetProperty(properties, "acquiring.api.proxy.username");
                var proxyPass = GetProperty(properties, "acquiring.api.proxy.password");
                if (proxyUri != null)
                {
                    ProxyUri = new Uri(proxyUri);
                    ProxyUserName = proxyUser;
                    ProxyPassword = proxyPass;
                }

                Integrator = GetProperty(properties, "acquiring.api.integrator", "");
            }
        }

        internal CommunicatorConfiguration(CommunicatorConfigurationSection section)
        {
            ApiEndpoint = section.ApiEndpoint;
            ConnectTimeout = section.ConnectTimeout;
            SocketTimeout = section.SocketTimeout;
            MaxConnections = section.MaxConnections;
            AuthorizationType = AuthorizationType.GetValueOf(section.AuthorizationType);
            AuthorizationId = section.AuthorizationId;
            AuthorizationSecret = section.AuthorizationSecret;
            OAuth2TokenUri = section.OAuth2TokenUri;

            ProxyUri = section.ProxyConfiguration.Uri;
            ProxyUserName = section.ProxyConfiguration.Username;
            ProxyPassword = section.ProxyConfiguration.Password;

            Integrator = section.Integrator;
            ShoppingCartExtension = section.ShoppingCartExtension;
        }

        /// <summary>
        /// Returns this with the API endpoint assigned.
        /// </summary>
        /// <param name="apiEndpoint">API endpoint.</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithApiEndpoint(Uri apiEndpoint)
        {
            ApiEndpoint = apiEndpoint;
            return this;
        }

        /// <summary>
        /// Returns this with the authorization id assigned.
        /// </summary>
        /// <param name="authorizationId">The authorization id</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithAuthorizationId(string authorizationId)
        {
            AuthorizationId = authorizationId;
            return this;
        }

        /// <summary>
        /// Returns this with the authorization secret assigned.
        /// </summary>
        /// <param name="authorizationSecret">The authorization secret</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithAuthorizationSecret(string authorizationSecret)
        {
            AuthorizationSecret = authorizationSecret;
            return this;
        }

        /// <summary>
        /// Returns this with the OAuth2 client id assigned.
        /// </summary>
        /// <param name="oauth2ClientId">The OAuth2 client id</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithOAuth2ClientId(string oauth2ClientId)
        {
            OAuth2ClientId = oauth2ClientId;
            return this;
        }

        /// <summary>
        /// Returns this with the OAuth2 client secret assigned.
        /// </summary>
        /// <param name="oauth2ClientSecret">The OAuth2 client secret</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithOAuth2ClientSecret(string oauth2ClientSecret)
        {
            OAuth2ClientSecret = oauth2ClientSecret;
            return this;
        }

        /// <summary>
        /// Returns this with the OAuth2 token URI assigned.
        /// </summary>
        /// <param name="oauth2TokenUri">The OAuth2 token URI</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithOAuth2TokenUri(string oauth2TokenUri)
        {
            OAuth2TokenUri = oauth2TokenUri;
            return this;
        }

        /// <summary>
        /// Returns this with the type of the authorization assigned.
        /// </summary>
        /// <param name="authorizationType">Authorization type.</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithAuthorizationType(AuthorizationType authorizationType)
        {
            AuthorizationType = authorizationType;
            return this;
        }

        /// <summary>
        /// Returns this with the the connect timeout assigned.
        /// </summary>
        /// <param name="connectTimeout">The connect timeout.</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithConnectTimeout(int connectTimeout)
        {
            ConnectTimeout = TimeSpan.FromMilliseconds(connectTimeout);
            return this;
        }

        /// <summary>
        /// Returns this with the the socket timeout assigned.
        /// </summary>
        /// <param name="socketTimeout">The socket timeout.</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithSocketTimeout(int socketTimeout)
        {
            SocketTimeout = TimeSpan.FromMilliseconds(socketTimeout);
            return this;
        }

        /// <summary>
        /// Returns this with the maximum number of connections assigned.
        /// </summary>
        /// <param name="maxConnections">The maximum number of connections.</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithMaxConnections(int maxConnections)
        {
            MaxConnections = maxConnections;
            return this;
        }

        /// <summary>
        /// Returns this with the proxy URI assigned.
        /// </summary>
        /// <param name="proxyUri">The proxy URI.</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithProxyUri(Uri proxyUri)
        {
            ProxyUri = proxyUri;
            return this;
        }

        /// <summary>
        /// Returns this with the proxy username assigned.
        /// </summary>
        /// <param name="proxyName">The proxy username.</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithProxyUserName(string proxyName)
        {
            ProxyUserName = proxyName;
            return this;
        }

        /// <summary>
        /// Returns this with the proxy password assigned.
        /// </summary>
        /// <param name="proxyPassword">The proxy password.</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithProxyPassword(string proxyPassword)
        {
            ProxyPassword = proxyPassword;
            return this;
        }

        /// <summary>
        /// Returns this with the integrator assigned.
        /// </summary>
        /// <param name="integrator">The integrator.</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithIntegrator(string integrator)
        {
            Integrator = integrator;
            return this;
        }

        /// <summary>
        /// Returns this with the shopping cart extension assigned.
        /// </summary>
        /// <param name="shoppingCartExtension">The shopping cart extension.</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithShoppingCartExtension(ShoppingCartExtension shoppingCartExtension)
        {
            ShoppingCartExtension = shoppingCartExtension;
            return this;
        }

        /// <summary>
        /// Returns this with a custom HttpClientHandler assigned.
        /// </summary>
        /// <param name="httpClientHandler">The custom HttpClientHandler.</param>
        /// <returns>This.</returns>
        public CommunicatorConfiguration WithHttpClientHandler(HttpClientHandler httpClientHandler)
        {
            HttpClientHandler = httpClientHandler;
            return this;
        }

        private static string GetProperty(IDictionary<string, string> properties, string name, string defaultValue = null)
        {
            return properties.TryGetValue(name, out var value) ? value : defaultValue;
        }

        private static int GetProperty(IDictionary<string, string> properties, string key, int defaultValue)
        {
            var propertyValue = GetProperty(properties, key);
            return int.TryParse(propertyValue, out var propertyInt) ? propertyInt : defaultValue;
        }

        private static Uri GetApiEndpoint(IDictionary<string, string> properties)
        {
            var host = GetProperty(properties, "acquiring.api.endpoint.host", "");
            var scheme = GetProperty(properties, "acquiring.api.endpoint.scheme", "https");
            var port = GetProperty(properties, "acquiring.api.endpoint.port", -1);

            return CreateUri(scheme, host, port);

        }

        private static Uri CreateUri(string scheme, string host, int port)
        {
            try
            {
                return new UriBuilder(scheme: scheme, host: host, portNumber: port).Uri;
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new ArgumentException("Unable to construct API endpoint URI", e);
            }
        }
    }
}
