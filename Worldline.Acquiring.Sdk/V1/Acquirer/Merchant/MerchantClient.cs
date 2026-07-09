/*
 * This file was automatically generated.
 */
using System.Collections.Generic;
using Worldline.Acquiring.Sdk.V1.Acquirer.Merchant.Accountverifications;
using Worldline.Acquiring.Sdk.V1.Acquirer.Merchant.Balanceinquiries;
using Worldline.Acquiring.Sdk.V1.Acquirer.Merchant.Cardpayments;
using Worldline.Acquiring.Sdk.V1.Acquirer.Merchant.Cardrefunds;
using Worldline.Acquiring.Sdk.V1.Acquirer.Merchant.Dynamiccurrencyconversion;
using Worldline.Acquiring.Sdk.V1.Acquirer.Merchant.Technicalreversals;

namespace Worldline.Acquiring.Sdk.V1.Acquirer.Merchant
{
    /// <summary>
    /// Merchant client. Thread-safe.
    /// </summary>
    public class MerchantClient : ApiResource
    {
        public MerchantClient(ApiResource parent, IDictionary<string, string> pathContext) :
            base(parent, pathContext)
        {
        }

        /// <summary>
        /// Resource /processing/v1/{acquirerId}/{merchantId}/payments
        /// </summary>
        /// <returns>CardPaymentsClient</returns>
        public CardPaymentsClient CardPayments => new CardPaymentsClient(this, null);

        /// <summary>
        /// Resource /processing/v1/{acquirerId}/{merchantId}/refunds
        /// </summary>
        /// <returns>CardRefundsClient</returns>
        public CardRefundsClient CardRefunds => new CardRefundsClient(this, null);

        /// <summary>
        /// Resource /processing/v1/{acquirerId}/{merchantId}/account-verifications
        /// </summary>
        /// <returns>AccountVerificationsClient</returns>
        public AccountVerificationsClient AccountVerifications => new AccountVerificationsClient(this, null);

        /// <summary>
        /// Resource /processing/v1/{acquirerId}/{merchantId}/balance-inquiries
        /// </summary>
        /// <returns>BalanceInquiriesClient</returns>
        public BalanceInquiriesClient BalanceInquiries => new BalanceInquiriesClient(this, null);

        /// <summary>
        /// Resource /processing/v1/{acquirerId}/{merchantId}/operations/{operationId}/reverse
        /// </summary>
        /// <returns>TechnicalReversalsClient</returns>
        public TechnicalReversalsClient TechnicalReversals => new TechnicalReversalsClient(this, null);

        /// <summary>
        /// Resource /services/v1/{acquirerId}/{merchantId}/dcc-rates
        /// </summary>
        /// <returns>DynamicCurrencyConversionClient</returns>
        public DynamicCurrencyConversionClient DynamicCurrencyConversion => new DynamicCurrencyConversionClient(this, null);
    }
}
