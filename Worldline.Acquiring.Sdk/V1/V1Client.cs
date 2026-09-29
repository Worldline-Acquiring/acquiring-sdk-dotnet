/*
 * This file was automatically generated.
 */
using System.Collections.Generic;
using Worldline.Acquiring.Sdk.V1.Acquirer;
using Worldline.Acquiring.Sdk.V1.Disputedocuments;
using Worldline.Acquiring.Sdk.V1.Disputeentries;
using Worldline.Acquiring.Sdk.V1.Disputemanagement;
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

        /// <summary>
        /// Resource /dispute-management/v1/disputes/search
        /// </summary>
        /// <returns>DisputeManagementClient</returns>
        public DisputeManagementClient DisputeManagement => new DisputeManagementClient(this, null);

        /// <summary>
        /// Resource /dispute-management/v1/documents
        /// </summary>
        /// <returns>DisputeDocumentsClient</returns>
        public DisputeDocumentsClient DisputeDocuments => new DisputeDocumentsClient(this, null);

        /// <summary>
        /// Resource /dispute-management/v1/dispute-entries/search
        /// </summary>
        /// <returns>DisputeEntriesClient</returns>
        public DisputeEntriesClient DisputeEntries => new DisputeEntriesClient(this, null);
    }
}
