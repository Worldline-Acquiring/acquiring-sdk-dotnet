/*
 * This file was automatically generated.
 */
using System.Collections.Generic;
using Worldline.Acquiring.Sdk.V1.Acquirer.Merchant;

namespace Worldline.Acquiring.Sdk.V1.Acquirer
{
    /// <summary>
    /// Acquirer client. Thread-safe.
    /// </summary>
    public class AcquirerClient : ApiResource
    {
        public AcquirerClient(ApiResource parent, IDictionary<string, string> pathContext) :
            base(parent, pathContext)
        {
        }

        /// <summary>
        /// Resource /processing/v1/{acquirerId}/{merchantId}
        /// </summary>
        /// <param name="merchantId">string</param>
        /// <returns>MerchantClient</returns>
        public MerchantClient WithNewMerchant(string merchantId)
        {
            var subContext = new Dictionary<string, string>
            {
                { "merchantId", merchantId }
            };
            return new MerchantClient(this, subContext);
        }
    }
}
