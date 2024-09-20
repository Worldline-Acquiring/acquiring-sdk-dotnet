/*
 * This file was automatically generated.
 */
using System.Collections.Generic;
using Worldline.Acquiring.Sdk.Communication;

namespace Worldline.Acquiring.Sdk.V1.Acquirer.Merchant.Payments
{
    /// <summary>
    /// Query parameters for
    /// <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Payments/operation/getPaymentStatus">Retrieve payment</a>
    /// </summary>
    public class GetPaymentStatusParams : AbstractParamRequest
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
