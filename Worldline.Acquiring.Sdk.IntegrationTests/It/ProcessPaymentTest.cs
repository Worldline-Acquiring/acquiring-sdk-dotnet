using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.Util;
using Worldline.Acquiring.Sdk.V1.Acquirer.Merchant.Payments;

namespace Worldline.Acquiring.Sdk.It
{
    public class ProcessPaymentTest : IntegrationTest
    {
        /// <summary>
        /// Smoke Test for process payment.
        /// </summary>
        [TestCase]
        public async Task Test()
        {
            using (var client = GetClient())
            {
                var paymentsClient = client.V1.WithNewAcquirer(GetAcquirerId()).WithNewMerchant(GetMerchantId()).Payments;

                var request = GetApiPaymentRequest();
                var response = await paymentsClient.ProcessPayment(request);
                AssertProcessPaymentResponse(request, response);

                var paymentId = response.PaymentId;

                var query = new GetPaymentStatusParams
                {
                    ReturnOperations = true
                };

                var status = await paymentsClient.GetPaymentStatus(paymentId, query);
                AssertPaymentStatusResponse(paymentId, status);
            }
        }
    }
}
