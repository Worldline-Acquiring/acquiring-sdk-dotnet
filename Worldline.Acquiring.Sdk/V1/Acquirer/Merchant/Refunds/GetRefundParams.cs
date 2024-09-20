/*
 * This file was automatically generated.
 */
using System.Collections.Generic;
using Worldline.Acquiring.Sdk.Communication;

namespace Worldline.Acquiring.Sdk.V1.Acquirer.Merchant.Refunds
{
    /// <summary>
    /// Query parameters for
    /// <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Refunds/operation/getRefund">Retrieve refund</a>
    /// </summary>
    public class GetRefundParams : AbstractParamRequest
    {
        /// <summary>
        /// If true, the response will contain the operations of the payment.
        /// False by default.
        /// </summary>
        public bool? ReturnOperations { get; set; }

        public override IEnumerable<RequestParam> ToRequestParameters()
        {
            var result = new List<RequestParam>();
            if (ReturnOperations != null)
            {
                result.Add(new RequestParam("returnOperations", ReturnOperations.ToString().ToLower()));
            }
            return result;
        }
    }
}
