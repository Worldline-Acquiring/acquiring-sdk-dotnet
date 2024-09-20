using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.Util;

namespace Worldline.Acquiring.Sdk.It
{
    public class SdkProxyTest : IntegrationTest
    {
        /// <summary>
        /// Smoke Test for using a proxy configured through SDK properties.
        /// </summary>
        public async Task Test()
        {
            using (var client = GetClient())
            {
                var dynamicCurrencyConversionClient = client.V1.WithNewAcquirer(GetAcquirerId()).WithNewMerchant(GetMerchantId()).DynamicCurrencyConversion;

                var configuration = GetCommunicatorConfiguration();
                Assert.NotNull(configuration.Proxy);
                AssertProxyAndAuthentication(GetConnectionFromService(dynamicCurrencyConversionClient), configuration.Proxy);

                var request = GetDCCRateRequest();
                var response = await dynamicCurrencyConversionClient.RequestDccRate(request);
                AssertDccRateResponse(request, response);
            }
        }

        private static DefaultConnection GetConnectionFromService(ApiResource services)
        {
            var communicator = (Communicator)services.GetPrivateField("_communicator");
            return communicator.GetPrivateProperty<DefaultConnection>("Connection");
        }

        private static void AssertProxyAndAuthentication(DefaultConnection connection, Proxy proxy)
        {
            var httpClient = (HttpClient)connection.GetPrivateField("_httpClient");
            var handler = (HttpClientHandler)httpClient.GetPrivateField<HttpMessageInvoker>("handler");
            Assert.That(handler, Is.Not.Null);
            Assert.That(handler.UseProxy, Is.True);
            Assert.That(((WebProxy)handler.Proxy)?.Address, Is.EqualTo(proxy.Uri));
            Assert.That((NetworkCredential)handler.Proxy?.Credentials, Is.Null);
            Assert.That(((NetworkCredential)handler.Proxy?.Credentials)?.UserName, Is.EqualTo(proxy.Username));
            Assert.That(((NetworkCredential)handler.Proxy?.Credentials)?.Password, Is.EqualTo(proxy.Password));
        }
    }
}
