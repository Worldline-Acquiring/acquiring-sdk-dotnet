using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Communication;

namespace Worldline.Acquiring.Sdk.It
{
    /// <summary>
    /// Verifies the <c>...Async</c> methods and cancelling an in-flight call against the Worldline Acquiring platform.
    /// Like the other tests that derive from <see cref="IntegrationTest"/>, these need the credentials in the environment variables.
    /// </summary>
    [TestFixture]
    public class CancellationTest : IntegrationTest
    {
        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(30);

        [TestCase]
        public async Task TestAsyncMethodWithoutACancelledToken()
        {
            using var client = GetClient();
            using var cts = new CancellationTokenSource(TestTimeout);
            var body = GetDCCRateRequest();

            var response = await client.V1.WithNewAcquirer(GetAcquirerId()).WithNewMerchant(GetMerchantId())
                .DynamicCurrencyConversion.RequestDccRateAsync(body, cancellationToken: cts.Token);

            AssertDccRateResponse(body, response);
        }

        [TestCase]
        public void TestCancelAnInFlightCall()
        {
            using var client = GetClient();
            using var cts = new CancellationTokenSource();
            var call = client.V1.WithNewAcquirer(GetAcquirerId()).WithNewMerchant(GetMerchantId())
                .DynamicCurrencyConversion.RequestDccRateAsync(GetDCCRateRequest(), cancellationToken: cts.Token);
            cts.Cancel();

            var exception = Assert.CatchAsync<OperationCanceledException>(async () => await WithTimeout(call));
            Assert.That(exception, Is.Not.InstanceOf<CommunicationException>());
        }

        [TestCase]
        public void TestCancelBeforeTheCall()
        {
            using var client = GetClient();
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            Assert.CatchAsync<OperationCanceledException>(async () => await WithTimeout(
                client.V1.WithNewAcquirer(GetAcquirerId()).WithNewMerchant(GetMerchantId())
                    .DynamicCurrencyConversion.RequestDccRateAsync(GetDCCRateRequest(), cancellationToken: cts.Token)));
        }

        private static async Task WithTimeout(Task task)
        {
            var finished = await Task.WhenAny(task, Task.Delay(TestTimeout));
            if (finished != task)
            {
                throw new TimeoutException("the call did not complete");
            }
            await task;
        }
    }
}
