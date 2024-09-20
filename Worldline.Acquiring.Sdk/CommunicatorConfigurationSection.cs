using System;
using System.Configuration;
using Worldline.Acquiring.Sdk.Domain;

namespace Worldline.Acquiring.Sdk
{
    internal class CommunicatorConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// The default number of maximum connections
        /// </summary>
        public const int DefaultMaxConnections = 10;

        [ConfigurationProperty("apiEndpoint", IsRequired = true)]
        public UriConfiguration ApiEndpointConfig => (UriConfiguration)this["apiEndpoint"];

        public Uri ApiEndpoint => ApiEndpointConfig.Uri;

        [ConfigurationProperty("connectTimeout", IsRequired = true)]
        public int ConnectTimeoutProperty => (int)this["connectTimeout"];

        public TimeSpan ConnectTimeout => TimeSpan.FromMilliseconds(ConnectTimeoutProperty);

        [ConfigurationProperty("socketTimeout", IsRequired = true)]
        public int SocketTimeoutProperty => (int)this["socketTimeout"];

        public TimeSpan SocketTimeout => TimeSpan.FromMilliseconds(SocketTimeoutProperty);

        [ConfigurationProperty("maxConnections", IsRequired = false, DefaultValue = DefaultMaxConnections)]
        public int MaxConnections => (int)this["maxConnections"];

        [ConfigurationProperty("authorizationType", IsRequired = true)]
        public string AuthorizationType => (string)this["authorizationType"];

        [ConfigurationProperty("authorizationId", IsRequired = false)]
        public string AuthorizationId => ((string)this["authorizationId"]).NullIfEmpty();

        [ConfigurationProperty("authorizationSecret", IsRequired = false)]
        public string AuthorizationSecret => ((string)this["authorizationSecret"]).NullIfEmpty();

        [ConfigurationProperty("oauth2TokenUri", IsRequired = false)]
        public string OAuth2TokenUri => ((string)this["oauth2TokenUri"]).NullIfEmpty();

        [ConfigurationProperty("integrator", IsRequired = true)]
        public string Integrator => ((string)this["integrator"]).NullIfEmpty();

        public ShoppingCartExtension ShoppingCartExtension => ShoppingCartExtensionConfiguration.ShoppingCartExtension;

        [ConfigurationProperty("shoppingCartExtension", IsRequired = false)]
        public ShoppingCartExtensionConfiguration ShoppingCartExtensionConfiguration => (ShoppingCartExtensionConfiguration)this["shoppingCartExtension"];

        [ConfigurationProperty("proxy", IsRequired = false)]
        public ProxyConfiguration ProxyConfiguration => this["proxy"] as ProxyConfiguration;
    }
}
