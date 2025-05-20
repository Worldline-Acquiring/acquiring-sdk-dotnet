using NUnit.Framework;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Authentication;
using Worldline.Acquiring.Sdk.V1;

namespace Worldline.Acquiring.Sdk.It
{
    public class CustomOAuth2ScopesTest : IntegrationTest
    {
        [TestCase("processing_dcc_rate")]
        [TestCase("processing_dcc_rate services_ping")]
        [TestCase("")]
        [TestCase(null)]
        public async Task TestWithValidScopes(string oauth2Scopes)
        {
            var configuration = GetCommunicatorConfiguration().WithOAuth2Scopes(oauth2Scopes);
            using (var client = Factory.CreateClient(configuration))
            {
                var request = GetDCCRateRequest();
                var response = await client.V1.WithNewAcquirer(GetAcquirerId()).WithNewMerchant(GetMerchantId()).DynamicCurrencyConversion.RequestDccRate(request);
                AssertDccRateResponse(request, response);
            }
        }

        [TestCase]
        public void TestWithMissingScopes()
        {
            var configuration = GetCommunicatorConfiguration().WithOAuth2Scopes("services_ping");
            using (var client = Factory.CreateClient(configuration))
            {
                var request = GetDCCRateRequest();
                Assert.That(async () => await client.V1.WithNewAcquirer(GetAcquirerId()).WithNewMerchant(GetMerchantId()).DynamicCurrencyConversion.RequestDccRate(request),
                    Throws.Exception.TypeOf(typeof(AuthorizationException)));
            }
        }

        [TestCase]
        public void TestWithInvalidScope()
        {
            var configuration = GetCommunicatorConfiguration().WithOAuth2Scopes("processing_dcc_rate invalid_scope");
            using (var client = Factory.CreateClient(configuration))
            {
                var request = GetDCCRateRequest();
                Assert.That(async () => await client.V1.WithNewAcquirer(GetAcquirerId()).WithNewMerchant(GetMerchantId()).DynamicCurrencyConversion.RequestDccRate(request),
                    Throws.Exception.TypeOf(typeof(OAuth2Exception))
                        .And.Message.StartsWith("There was an error while retrieving the OAuth2 access token: invalid_scope - "));
            }
        }
    }
}
