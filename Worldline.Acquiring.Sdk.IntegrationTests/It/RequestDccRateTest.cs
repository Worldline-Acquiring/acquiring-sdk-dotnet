using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.Util;

namespace Worldline.Acquiring.Sdk.It
{
    public class RequestDccRateTest : IntegrationTest
    {
        /// <summary>
        /// Smoke Test for request DCC rate.
        /// </summary>
        [TestCase]
        public async Task Test()
        {
            using (var client = GetClient())
            {
                var request = GetDCCRateRequest();
                var response = await client.V1.WithNewAcquirer(GetAcquirerId()).WithNewMerchant(GetMerchantId()).DynamicCurrencyConversion.RequestDccRate(request);
                AssertDccRateResponse(request, response);
            }
        }
    }
}
