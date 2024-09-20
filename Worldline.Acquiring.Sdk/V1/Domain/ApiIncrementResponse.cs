/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class ApiIncrementResponse : ApiActionResponse
    {
        /// <summary>
        /// Authorization approval code
        /// </summary>
        public string AuthorizationCode { get; set; }

        /// <summary>
        /// Amount for the operation.
        /// </summary>
        public AmountData TotalAuthorizedAmount { get; set; }
    }
}
