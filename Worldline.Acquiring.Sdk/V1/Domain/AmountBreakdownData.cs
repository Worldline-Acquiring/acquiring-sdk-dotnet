/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class AmountBreakdownData
    {
        /// <summary>
        /// Optional amount of cashback for card-present transactions.
        /// <p />
        /// The amount specified is included in the total transaction <c>amount</c>, the information is provided
        /// for data enrichment and reconciliation purposes.
        /// <p />
        /// Only supported in some regions with restrictions, depending on local regulation and card scheme rules.
        /// Please check with your Worldline contact if you are allowed to use this field.
        /// </summary>
        public AmountData CashbackAmount { get; set; }

        /// <summary>
        /// Optional amount of tip.
        /// <p />
        /// The amount specified is included in the total transaction <c>amount</c>, the information is provided
        /// for data enrichment and reconciliation purposes.
        /// </summary>
        public AmountData TipAmount { get; set; }
    }
}
