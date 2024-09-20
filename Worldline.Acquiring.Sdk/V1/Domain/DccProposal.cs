/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DccProposal
    {
        /// <summary>
        /// Amount for the operation.
        /// </summary>
        public AmountData OriginalAmount { get; set; }

        public RateData Rate { get; set; }

        /// <summary>
        /// The rate reference ID
        /// </summary>
        public string RateReferenceId { get; set; }

        /// <summary>
        /// Amount for the operation.
        /// </summary>
        public AmountData ResultingAmount { get; set; }
    }
}
