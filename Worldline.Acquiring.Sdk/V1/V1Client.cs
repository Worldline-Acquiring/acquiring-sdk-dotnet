/*
 * This file was automatically generated.
 */
using System.Collections.Generic;
using Worldline.Acquiring.Sdk.V1.Acquirer;
using Worldline.Acquiring.Sdk.V1.Ping;

namespace Worldline.Acquiring.Sdk.V1
{
    /// <summary>
    /// V1. Thread-safe.
    /// </summary>
    public class V1Client : ApiResource
    {
        public V1Client(ApiResource parent, IDictionary<string, string> pathContext) :
            base(parent, pathContext)
        {
        }

        /// <summary>
        /// Resource /processing/v1/{acquirerId}
        /// </summary>
        /// <param name="acquirerId">string</param>
        /// <returns>AcquirerClient</returns>
        public AcquirerClient WithNewAcquirer(string acquirerId)
        {
            var subContext = new Dictionary<string, string>
            {
                { "acquirerId", acquirerId }
            };
            return new AcquirerClient(this, subContext);
        }

        /// <summary>
        /// Resource /services/v1/ping
        /// </summary>
        /// <returns>PingClient</returns>
        public PingClient Ping => new PingClient(this, null);
    }
}
