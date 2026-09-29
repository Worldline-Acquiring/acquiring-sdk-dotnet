/*
 * This file was automatically generated.
 */
using System.Collections.Generic;
using Worldline.Acquiring.Sdk.Communication;

namespace Worldline.Acquiring.Sdk.V1.Disputemanagement
{
    /// <summary>
    /// Query parameters for
    /// <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Dispute-Management/operation/getDispute">Retrieve Dispute</a>
    /// </summary>
    public class GetDisputeParams : AbstractParamRequest
    {
        /// <summary>
        /// If true, the response will include the full history of dispute entries related to the dispute.
        /// False by default.
        /// </summary>
        public bool? IncludeEntries { get; set; }

        public override IEnumerable<RequestParam> ToRequestParameters()
        {
            var result = new List<RequestParam>();
            if (IncludeEntries != null)
            {
                result.Add(new RequestParam("includeEntries", IncludeEntries.ToString().ToLower()));
            }
            return result;
        }
    }
}
