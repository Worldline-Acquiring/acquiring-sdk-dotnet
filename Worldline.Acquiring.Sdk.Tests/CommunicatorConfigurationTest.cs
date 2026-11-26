using NUnit.Framework;
using System;
using System.Collections.Generic;
using Worldline.Acquiring.Sdk.Authentication;

namespace Worldline.Acquiring.Sdk
{
    [TestFixture]
    public class CommunicatorConfigurationTest
    {
        private const string AuthType = "OAuth2";

        [TestCase]
        public void TestConstructFromPropertiesWithoutProxy()
        {
            var configuration = CreateBasicConfiguration();

            AssertBasicConfigurationSettings(configuration);
            Assert.That(configuration.MaxConnections, Is.EqualTo(CommunicatorConfiguration.DefaultMaxConnections));
            Assert.That(configuration.AuthorizationId, Is.Null);
            Assert.That(configuration.AuthorizationSecret, Is.Null);
            Assert.That(configuration.OAuth2Scopes, Is.Null);

            Assert.That(configuration.Proxy, Is.Null);
            Assert.That(configuration.ProxyUri, Is.Null);
            Assert.That(configuration.ProxyUserName, Is.Null);
            Assert.That(configuration.ProxyPassword, Is.Null);
        }

        [TestCase]
        public void TestConstructFromPropertiesWithProxyWithoutAuthentication()
        {
            var configuration = CreateBasicConfiguration()
                .WithProxyUri(new Uri("http://proxy.example.org:3128"));

            AssertBasicConfigurationSettings(configuration);
            Assert.That(configuration.MaxConnections, Is.EqualTo(CommunicatorConfiguration.DefaultMaxConnections));
            Assert.That(configuration.AuthorizationId, Is.Null);
            Assert.That(configuration.AuthorizationSecret, Is.Null);
            Assert.That(configuration.OAuth2Scopes, Is.Null);

            var proxy = configuration.Proxy;
            Assert.That(proxy, Is.Not.Null);
            AssertBasicProxySettings(proxy);
            Assert.That(proxy.Username, Is.Null);
            Assert.That(proxy.Password, Is.Null);
        }

        [TestCase]
        public void TestConstructFromPropertiesWithProxyWithAuthentication()
        {
            var configuration = CreateBasicConfiguration()
                .WithProxyUri(new Uri("http://proxy.example.org:3128"))
                .WithProxyUserName("proxy-username")
                .WithProxyPassword("proxy-password");

            AssertBasicConfigurationSettings(configuration);
            Assert.That(configuration.MaxConnections, Is.EqualTo(CommunicatorConfiguration.DefaultMaxConnections));
            Assert.That(configuration.AuthorizationId, Is.Null);
            Assert.That(configuration.AuthorizationSecret, Is.Null);
            Assert.That(configuration.OAuth2Scopes, Is.Null);

            var proxy = configuration.Proxy;
            Assert.That(proxy, Is.Not.Null);
            AssertBasicProxySettings(proxy);
            Assert.That(proxy.Username, Is.EqualTo("proxy-username"));
            Assert.That(proxy.Password, Is.EqualTo("proxy-password"));
        }

        [TestCase]
        public void TestConstructFromPropertiesWithMaxConnections()
        {
            var configuration = CreateBasicConfiguration()
                .WithMaxConnections(100);

            AssertBasicConfigurationSettings(configuration);
            Assert.That(configuration.MaxConnections, Is.EqualTo(100));
            Assert.That(configuration.AuthorizationId, Is.Null);
            Assert.That(configuration.AuthorizationSecret, Is.Null);
            Assert.That(configuration.OAuth2Scopes, Is.Null);
        }

        [TestCase]
        public void TestConstructFromPropertiesWithHostAndScheme()
        {
            var properties = new Dictionary<string, string>
            {
                ["acquiring.api.endpoint.scheme"] = "http",
                ["acquiring.api.endpoint.host"] = "api.preprod.acquiring.worldline-solutions.com",
                ["acquiring.api.authorizationType"] = AuthType,
                ["acquiring.api.connectTimeout"] = "20000",
                ["acquiring.api.socketTimeout"] = "10000"
            };

            var configuration = new CommunicatorConfiguration(properties);

            Assert.That(configuration.ApiEndpoint, Is.EqualTo(new Uri("http://api.preprod.acquiring.worldline-solutions.com")));
        }

        [TestCase]
        public void TestConstructFromPropertiesWithHostAndPort()
        {
            var properties = new Dictionary<string, string>
            {
                ["acquiring.api.endpoint.port"] = "8443",
                ["acquiring.api.endpoint.host"] = "api.preprod.acquiring.worldline-solutions.com",
                ["acquiring.api.authorizationType"] = AuthType,
                ["acquiring.api.connectTimeout"] = "20000",
                ["acquiring.api.socketTimeout"] = "10000"
            };

            var configuration = new CommunicatorConfiguration(properties);

            Assert.That(configuration.ApiEndpoint, Is.EqualTo(new Uri("https://api.preprod.acquiring.worldline-solutions.com:8443")));
        }

        [TestCase]
        public void TestConstructFromPropertiesWithHostSchemeAndPort()
        {
            var properties = new Dictionary<string, string>
            {
                ["acquiring.api.endpoint.port"] = "8080",
                ["acquiring.api.endpoint.host"] = "api.preprod.acquiring.worldline-solutions.com",
                ["acquiring.api.endpoint.scheme"] = "http",
                ["acquiring.api.authorizationType"] = AuthType,
                ["acquiring.api.connectTimeout"] = "20000",
                ["acquiring.api.socketTimeout"] = "10000"
            };

            var configuration = new CommunicatorConfiguration(properties);

            Assert.That(configuration.ApiEndpoint, Is.EqualTo(new Uri("http://api.preprod.acquiring.worldline-solutions.com:8080")));
        }

        [TestCase]
        public void TestConstructFromPropertiesWithIPv6Host()
        {
            var properties = new Dictionary<string, string>
            {
                ["acquiring.api.endpoint.host"] = "::1",
                ["acquiring.api.authorizationType"] = AuthType,
                ["acquiring.api.connectTimeout"] = "20000",
                ["acquiring.api.socketTimeout"] = "10000"
            };

            var configuration = new CommunicatorConfiguration(properties);

            Assert.That(configuration.ApiEndpoint, Is.EqualTo(new Uri("https://[::1]")));
        }

        [TestCase]
        public void TestCustomOAuth2Scopes()
        {
            var properties = new Dictionary<string, string>
            {
                ["acquiring.api.endpoint.port"] = "8080",
                ["acquiring.api.endpoint.host"] = "api.preprod.acquiring.worldline-solutions.com",
                ["acquiring.api.endpoint.scheme"] = "http",
                ["acquiring.api.authorizationType"] = AuthType,
                ["acquiring.api.oauth2.scopes"] = "processing_dcc_rate invalid_scope",
                ["acquiring.api.connectTimeout"] = "20000",
                ["acquiring.api.socketTimeout"] = "10000"
            };

            var configuration = new CommunicatorConfiguration(properties);

            Assert.That(configuration.OAuth2Scopes, Is.EqualTo("processing_dcc_rate invalid_scope"));
        }

        private static CommunicatorConfiguration CreateBasicConfiguration()
        {
            return new CommunicatorConfiguration()
                .WithApiEndpoint(new Uri("https://api.preprod.acquiring.worldline-solutions.com"))
                .WithAuthorizationType(AuthorizationType.OAuth2)
                .WithConnectTimeout(20000)
                .WithSocketTimeout(10000);
        }

        private static void AssertBasicConfigurationSettings(CommunicatorConfiguration configuration)
        {
            Assert.That(configuration.ApiEndpoint, Is.EqualTo(new Uri("https://api.preprod.acquiring.worldline-solutions.com")));
            Assert.That(configuration.AuthorizationType, Is.EqualTo(AuthorizationType.OAuth2));
            Assert.That(configuration.ConnectTimeout?.TotalMilliseconds, Is.EqualTo(20000));
            Assert.That(configuration.SocketTimeout?.TotalMilliseconds, Is.EqualTo(10000));
        }

        /// <summary>
        /// Checks validity of basic proxy settings of a proxy with uri http://proxy.example.org:3128.
        /// </summary>
        /// <param name="proxy">Proxy.</param>
        private static void AssertBasicProxySettings(Proxy proxy)
        {
            Assert.That(proxy.Uri.Scheme, Is.EqualTo("http"));
            Assert.That(proxy.Uri.Host, Is.EqualTo("proxy.example.org"));
            Assert.That(proxy.Uri.Port, Is.EqualTo(3128));
        }
    }
}
