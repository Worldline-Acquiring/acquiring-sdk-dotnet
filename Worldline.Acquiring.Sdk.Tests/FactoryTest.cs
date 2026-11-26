using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using Worldline.Acquiring.Sdk.Authentication;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.Json;
using Worldline.Acquiring.Sdk.Util;

namespace Worldline.Acquiring.Sdk
{
    [TestFixture]
    public class FactoryTest
    {
        public const string AuthorizationId = "someId";
        public const string AuthorizationSecret = "someSecret";

        public static readonly IDictionary<string, string> DictOAuth2 = new Dictionary<string, string>
        {
            { "acquiring.api.integrator", "Worldline" },
            { "acquiring.api.endpoint.host", "api.preprod.acquiring.worldline-solutions.com" },
            { "acquiring.api.authorizationType", "OAuth2" },
            { "acquiring.api.oauth2.tokenUri", "https://sso.preprod.acquiring.worldline-solutions.com/auth/realms/acquiring_api/protocol/openid-acquiring/token" },
            { "acquiring.api.socketTimeout", "10000" },
            { "acquiring.api.connectTimeout", "-1" },
            { "acquiring.api.maxConnections", "100" }
        };

        [TestCase]
        public void TestCreateConfiguration()
        {
            var configuration = Factory.CreateConfiguration(DictOAuth2, AuthorizationId, AuthorizationSecret);
            Assert.That(configuration.ApiEndpoint, Is.EqualTo(new Uri("https://api.preprod.acquiring.worldline-solutions.com")));
            Assert.That(configuration.AuthorizationType, Is.EqualTo(AuthorizationType.OAuth2));
            Assert.That(configuration.ConnectTimeout, Is.Null);
            Assert.That(configuration.SocketTimeout, Is.EqualTo(new TimeSpan(0, 0, 10)));
            Assert.That(configuration.MaxConnections, Is.EqualTo(100));
            Assert.That(configuration.OAuth2ClientId, Is.EqualTo(AuthorizationId));
            Assert.That(configuration.OAuth2ClientSecret, Is.EqualTo(AuthorizationSecret));
            Assert.That(configuration.OAuth2TokenUri, Is.EqualTo("https://sso.preprod.acquiring.worldline-solutions.com/auth/realms/acquiring_api/protocol/openid-acquiring/token"));
        }

        [TestCase]
        public void TestCreateCommunicator()
        {
            var communicator = Factory.CreateCommunicator(DictOAuth2, AuthorizationId, AuthorizationSecret);

            Assert.That(communicator.Marshaller, Is.SameAs(DefaultMarshaller.Instance));

            var connection = communicator.Connection;
            Assert.That(connection, Is.InstanceOf<DefaultConnection>());
            //DefaultConnectionTest.assertConnection((DefaultConnection) connection, -1, -1, 100, null);

            var authenticator = communicator.Authenticator;
            Assert.That(authenticator, Is.InstanceOf<OAuth2Authenticator>());
            Assert.That(authenticator.GetPrivateField(typeof(OAuth2Authenticator), "_clientId"), Is.EqualTo(AuthorizationId));
            Assert.That(authenticator.GetPrivateField(typeof(OAuth2Authenticator), "_clientSecret"), Is.EqualTo(AuthorizationSecret));

            var metadataProvider = communicator.MetadataProvider;
            Assert.That(metadataProvider.GetType(), Is.EqualTo(typeof(MetadataProvider)));
            var requestHeaders = metadataProvider.ServerMetadataHeaders;
            Assert.That(requestHeaders.Count(), Is.EqualTo(1));
            var requestHeader = requestHeaders.ElementAt(0);
            Assert.That(requestHeader.Name, Is.EqualTo("X-WL-ServerMetaInfo"));
        }
    }
}
