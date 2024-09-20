/*
 * This file was automatically generated.
 */
using System;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class TransactionDataForDcc
    {
        /// <summary>
        /// Amount for the operation.
        /// </summary>
        public AmountData Amount { get; set; }

        /// <summary>
        /// The date and time of the transaction
        /// </summary>
        public DateTimeOffset TransactionTimestamp { get; set; }

        /// <summary>
        /// The transaction type
        /// </summary>
        public string TransactionType { get; set; }
    }
}
