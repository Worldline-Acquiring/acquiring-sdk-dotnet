using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Worldline.Acquiring.Sdk.It
{
    public class ConnectionPoolingTest : IntegrationTest
    {
        [TestCase(10, 10)]
        [TestCase(10, 5)]
        [TestCase(10, 1)]
        public async Task TestConnectionPooling(int requestCount, int maxConnections)
        {
            var configuration = GetCommunicatorConfiguration().WithMaxConnections(maxConnections);

            using (var communicator = Factory.CreateCommunicator(configuration))
            {
                await ActuallyTestConnectionPooling(communicator, requestCount);
            }
        }

        private static async Task ActuallyTestConnectionPooling(Communicator communicator, int requestCount)
        {
            await Task.WhenAll(Enumerable.Range(0, requestCount)
                               .Select(requestNum =>
                                          Factory.CreateClient(communicator)
                                              .V1
                                              .WithNewAcquirer(GetAcquirerId())
                                              .WithNewMerchant(GetMerchantId())
                                              .DynamicCurrencyConversion
                                              .RequestDccRate(GetDCCRateRequest(200 * requestNum))
                                     ).ToList()
                              );
        }
    }
}
