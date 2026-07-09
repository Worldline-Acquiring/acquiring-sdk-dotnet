using NUnit.Framework;
using System.Threading.Tasks;

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
