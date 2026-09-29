/*
 * This file was automatically generated.
 */
using System;
using Newtonsoft.Json;
using Worldline.Acquiring.Sdk.Json;

namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DateRange
    {
        /// <summary>
        /// A date value that can be used in search criteria to filter results to only include items
        /// with a date greater than or equal to the specified value (equal or after). The date is in ISO 8601 format.
        /// </summary>
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? GreaterEqual { get; set; }

        /// <summary>
        /// A date value that can be used in search criteria to filter results to only include items
        /// with a date lower than the specified value (equal or before). The date is in ISO 8601 format.
        /// </summary>
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? LowerEqual { get; set; }
    }
}
