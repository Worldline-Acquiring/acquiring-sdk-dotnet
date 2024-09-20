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
            Assert.AreEqual(new Uri("https://api.preprod.acquiring.worldline-solutions.com"), configuration.ApiEndpoint);
            Assert.AreEqual(AuthorizationType.OAuth2, configuration.AuthorizationType);
            Assert.AreEqual(null, configuration.ConnectTimeout);
            Assert.AreEqual(new TimeSpan(0, 0, 10), configuration.SocketTimeout);
            Assert.AreEqual(100, configuration.MaxConnections);
            Assert.AreEqual(AuthorizationId, configuration.OAuth2ClientId);
            Assert.AreEqual(AuthorizationSecret, configuration.OAuth2ClientSecret);
            Assert.AreEqual("https://sso.preprod.acquiring.worldline-solutions.com/auth/realms/acquiring_api/protocol/openid-acquiring/token", configuration.OAuth2TokenUri);
        }

        [TestCase]
        public void TestCreateCommunicator()
        {
            var communicator = Factory.CreateCommunicator(DictOAuth2, AuthorizationId, AuthorizationSecret);

            Assert.AreSame(DefaultMarshaller.Instance, communicator.Marshaller);

            var connection = communicator.Connection;
            Assert.True(connection is DefaultConnection);
            //DefaultConnectionTest.assertConnection((DefaultConnection) connection, -1, -1, 100, null);

            var authenticator = communicator.Authenticator;
            Assert.True(authenticator is OAuth2Authenticator);
            Assert.AreEqual(AuthorizationId, authenticator.GetPrivateField(typeof(OAuth2Authenticator), "_clientId"));
            Assert.AreEqual(AuthorizationSecret, authenticator.GetPrivateField(typeof(OAuth2Authenticator), "_clientSecret"));

            var metadataProvider = communicator.MetadataProvider;
            Assert.AreEqual(typeof(MetadataProvider), metadataProvider.GetType());
            var requestHeaders = metadataProvider.ServerMetadataHeaders;
            Assert.AreEqual(1, requestHeaders.Count());
            var requestHeader = requestHeaders.ElementAt(0);
            Assert.AreEqual("X-WL-ServerMetaInfo", requestHeader.Name);
        }
    }
}
