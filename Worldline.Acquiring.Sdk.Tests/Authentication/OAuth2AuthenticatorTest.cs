using NUnit.Framework;
using System;
using MockHttpServer;
using System.Threading.Tasks;
using System.Threading;

namespace Worldline.Acquiring.Sdk.Authentication
{
    [TestFixture]
    public class OAuth2AuthenticatorTest
    {
        private const int Port = 5357;

        private const string OAuth2AccessTokenResponse = @"{
  ""access_token"": ""accessToken"",
  ""expires_in"": 300
}";

        private const string OAuth2AccessTokenExpiredResponse = @"{
  ""access_token"": ""expiredAccessToken"",
  ""expires_in"": -1
}";

        private const string OAuth2AccessTokenInvalidClientResponse = @"{
  ""error"": ""unauthorized_client"",
  ""error_description"": ""INVALID_CREDENTIALS: Invalid client credentials""
}";

        private static readonly string ClientId = Guid.NewGuid().ToString();
        private static readonly string ClientSecret = Guid.NewGuid().ToString();
        private static readonly TimeSpan SocketTimeout = TimeSpan.FromSeconds(10);
        private const string OAuth2TokenUri = "/auth/realms/api/protocol/openid-connect/token";
        private static readonly Uri Path = new UriBuilder("http://domain.local/api/v1/payments").Uri;

        private CommunicatorConfiguration _communicatorConfiguration;

        [SetUp]
        public void Setup()
        {
            _communicatorConfiguration = new CommunicatorConfiguration
            {
                OAuth2ClientId = ClientId,
                OAuth2ClientSecret = ClientSecret,
                OAuth2TokenUri = "http://localhost:" + Port + OAuth2TokenUri,
                SocketTimeout = SocketTimeout
            };
        }

        [TestCase]
        public async Task TestGetAuthorization()
        {
            var invocationCount = 0;

            using (var _ = new MockServer(Port, OAuth2TokenUri, (request, response, arg3) =>
                   {
                       Interlocked.Increment(ref invocationCount);

                       response.StatusCode = 200;
                       response.Headers.Add("Content-Type", "application/json");

                       return OAuth2AccessTokenResponse;
                   }))
            {
                var authenticator = new OAuth2Authenticator(_communicatorConfiguration);

                var authorization = await authenticator.GetAuthorization(null, Path, null);

                Assert.That(authorization, Is.EqualTo("Bearer accessToken"));
                Assert.That(invocationCount, Is.EqualTo(1));
            }
        }

        [TestCase]
        public void TestGetAuthorizationWithInvalidClientId()
        {
            var invocationCount = 0;

            using (var _ = new MockServer(Port, OAuth2TokenUri, (request, response, arg3) =>
                   {
                       Interlocked.Increment(ref invocationCount);

                       response.StatusCode = 401;
                       response.Headers.Add("Content-Type", "application/json");

                       return OAuth2AccessTokenInvalidClientResponse;
                   }))
            {
                var authenticator = new OAuth2Authenticator(_communicatorConfiguration);

                Assert.That(async () => await authenticator.GetAuthorization(null, Path, null),
                    Throws.Exception.TypeOf(typeof(OAuth2Exception))
                    .And.Message.EqualTo("There was an error while retrieving the OAuth2 access token: unauthorized_client - INVALID_CREDENTIALS: Invalid client credentials"));
                Assert.That(invocationCount, Is.EqualTo(1));
                Assert.That(invocationCount, Is.EqualTo(1));
            }
        }

        [TestCase]
        public void TestGetAuthorizationWithMultipleThreadsAndValidToken()
        {
            var invocationCount = 0;
            var startEvent = new CountdownEvent(1);
            var readyEvent = new CountdownEvent(10);
            var finishEvent = new CountdownEvent(10);

            using (var _ = new MockServer(Port, OAuth2TokenUri, (request, response, arg3) =>
                    {
                        Interlocked.Increment(ref invocationCount);

                        response.StatusCode = 200;
                        response.Headers.Add("Content-Type", "application/json");

                        return OAuth2AccessTokenResponse;
                    }))
            {
                var authenticator = new OAuth2Authenticator(_communicatorConfiguration);

                for (var i = 0; i < 10; i++)
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    Task.Factory.StartNew(async () =>
                            {
                                try
                                {
                                    readyEvent.Signal();
                                    startEvent.Wait();
                                    await authenticator.GetAuthorization(null, Path, null);
                                }
                                finally
                                {
                                    finishEvent.Signal();
                                }
                            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }

                readyEvent.Wait();
                startEvent.Signal();
                finishEvent.Wait();

                Assert.That(invocationCount, Is.EqualTo(1));
            }
        }

        [TestCase]
        public void TestGetAuthorizationWithMultipleThreadsAndExpiredToken()
        {
            var invocationCount = 0;
            var startEvent = new CountdownEvent(1);
            var readyEvent = new CountdownEvent(10);
            var finishEvent = new CountdownEvent(10);

            using (var _ = new MockServer(Port, OAuth2TokenUri, (request, response, arg3) =>
                    {
                        Interlocked.Increment(ref invocationCount);

                        response.StatusCode = 200;
                        response.Headers.Add("Content-Type", "application/json");

                        return OAuth2AccessTokenExpiredResponse;
                    }))
            {
                var authenticator = new OAuth2Authenticator(_communicatorConfiguration);

                for (var i = 0; i < 10; i++)
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    Task.Factory.StartNew(async () =>
                            {
                                try
                                {
                                    readyEvent.Signal();
                                    startEvent.Wait();
                                    await authenticator.GetAuthorization(null, Path, null);
                                }
                                finally
                                {
                                    finishEvent.Signal();
                                }
                            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }

                readyEvent.Wait();
                startEvent.Signal();
                finishEvent.Wait();

                Assert.That(invocationCount, Is.EqualTo(10));
            }
        }
    }
}
