using System;
using System.Globalization;
using NUnit.Framework;
using Worldline.Acquiring.Sdk.V1.Domain;

namespace Worldline.Acquiring.Sdk.It
{
    public abstract class IntegrationTest
    {
        protected static CommunicatorConfiguration GetCommunicatorConfiguration()
        {
            if (OAuth2ClientId != null && OAuth2ClientSecret != null && OAuth2TokenUri != null)
            {
                var configuration = Factory.CreateConfiguration(OAuth2ClientId, OAuth2ClientSecret)
                    .WithOAuth2TokenUri(OAuth2TokenUri);
                if (ApiEndpointUri != null)
                {
                    configuration.ApiEndpoint = new Uri(ApiEndpointUri);
                }
                return configuration;
            }
            throw new InvalidOperationException("Environment variables acquiring_api_oauth2_clientId, acquiring_api_oauth2_clientSecret and acquiring_api_oauth2_tokenUri must be set");
        }

        private static readonly string OAuth2ClientId = Environment.GetEnvironmentVariable("acquiring_api_oauth2_clientId");
        private static readonly string OAuth2ClientSecret = Environment.GetEnvironmentVariable("acquiring_api_oauth2_clientSecret");
        private static readonly string OAuth2TokenUri = Environment.GetEnvironmentVariable("acquiring_api_oauth2_tokenUri");
        private static readonly string MerchantId = Environment.GetEnvironmentVariable("acquiring_api_merchantId");
        private static readonly string AcquirerId = Environment.GetEnvironmentVariable("acquiring_api_acquirerId");
        private static readonly string ApiEndpointUri = Environment.GetEnvironmentVariable("acquiring_api_endpointUri");

        protected static Client GetClient()
        {
            var configuration = GetCommunicatorConfiguration();
            return Factory.CreateClient(configuration);
        }

        protected static string GetMerchantId()
        {
            if (MerchantId != null)
            {
                return MerchantId;
            }
            throw new InvalidOperationException("Environment variable acquiring_api_merchantId must be set");
        }

        protected static string GetAcquirerId()
        {
            if (AcquirerId != null)
            {
                return AcquirerId;
            }
            throw new InvalidOperationException("Environment variable acquiring_api_acquirerId must be set");
        }

        protected static ApiPaymentRequest GetApiPaymentRequest(long amount = 200)
        {
            var body = new ApiPaymentRequest
            {
                Amount = new AmountData
                {
                    Amount = amount,
                    CurrencyCode = "GBP",
                    NumberOfDecimals = 2
                },
                AuthorizationType = "PRE_AUTHORIZATION",
                TransactionTimestamp = DateTimeOffset.UtcNow,
                CardPaymentData = new CardPaymentData
                {
                    CardEntryMode = "ECOMMERCE",
                    AllowPartialApproval = false,
                    CardholderVerificationMethod = "CARD_SECURITY_CODE",
                    Brand = "VISA",
                    CaptureImmediately = false,
                    CardData = new PlainCardData
                    {
                        CardNumber = "4176669999000104",
                        CardSecurityCode = "012",
                        ExpiryDate = "122031"
                    }
                },
                References = new PaymentReferences
                {
                    MerchantReference = "your-order" + Guid.NewGuid()
                },
                OperationId = Guid.NewGuid().ToString()
            };
            return body;
        }

        protected static void AssertProcessPaymentResponse(ApiPaymentRequest body, ApiPaymentResponse apiPaymentResponse)
        {
            Assert.AreEqual(body.OperationId, apiPaymentResponse.OperationId);
            Assert.AreEqual("0", apiPaymentResponse.ResponseCode);
            Assert.AreEqual("APPROVED", apiPaymentResponse.ResponseCodeCategory);
            Assert.NotNull(apiPaymentResponse.ResponseCodeDescription);
            Assert.AreEqual("AUTHORIZED", apiPaymentResponse.Status);
            Assert.NotNull(apiPaymentResponse.InitialAuthorizationCode);
            Assert.NotNull(apiPaymentResponse.PaymentId);
            Assert.NotNull(apiPaymentResponse.TotalAuthorizedAmount);
            Assert.AreEqual(200L, apiPaymentResponse.TotalAuthorizedAmount.Amount);
            Assert.AreEqual("GBP", apiPaymentResponse.TotalAuthorizedAmount.CurrencyCode);
            Assert.AreEqual(2, apiPaymentResponse.TotalAuthorizedAmount.NumberOfDecimals);
        }

        protected static void AssertPaymentStatusResponse(string paymentId, ApiPaymentResource response)
        {
            Assert.NotNull(response.InitialAuthorizationCode);
            Assert.AreEqual(paymentId, response.PaymentId);
            Assert.AreEqual("AUTHORIZED", response.Status);
        }

        protected static GetDCCRateRequest GetDCCRateRequest(long amount = 200)
        {
            var body = new GetDCCRateRequest
            {
                OperationId = Guid.NewGuid().ToString(),
                TargetCurrency = "EUR",
                CardPaymentData = new CardDataForDcc
                {
                    Bin = "41766699",
                    Brand = "VISA"
                },
                PointOfSaleData = new PointOfSaleDataForDcc
                {
                    TerminalId = "12345678"
                },
                Transaction = new TransactionDataForDcc
                {
                    Amount = new AmountData
                    {
                        Amount = amount,
                        CurrencyCode = "GBP",
                        NumberOfDecimals = 2
                    },
                    TransactionType = "PAYMENT",
                    TransactionTimestamp = DateTimeOffset.Now
                }
            };
            return body;
        }

        protected static void AssertDccRateResponse(GetDCCRateRequest body, GetDccRateResponse response)
        {
            Assert.NotNull(response.Proposal);
            Assert.NotNull(response.Proposal.OriginalAmount);
            AssertEqualAmounts(body.Transaction.Amount, response.Proposal.OriginalAmount);
            Assert.AreEqual(body.TargetCurrency, response.Proposal.ResultingAmount.CurrencyCode);
        }

        protected static void AssertEqualAmounts(AmountData expected, AmountData actual) {
            Assert.AreEqual(expected.Amount, actual.Amount);
            Assert.AreEqual(expected.CurrencyCode, actual.CurrencyCode);
            Assert.AreEqual(expected.NumberOfDecimals, actual.NumberOfDecimals);
        }
    }
}
