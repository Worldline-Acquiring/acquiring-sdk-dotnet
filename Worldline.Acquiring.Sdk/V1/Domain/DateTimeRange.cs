/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DateTimeRange
    {
        /// <summary>
        /// A date-time value that can be used in search criteria to filter results to only include items
        /// with a date-time greater to the specified value (after). The date-time is in ISO 8601 format, but
        /// without the timezone designator.
        /// </summary>
        public string Greater { get; set; }

        /// <summary>
        /// A date-time value that can be used in search criteria to filter results to only include items
        /// with a date-time greater than or equal to the specified value (equal or after). The date-time is in ISO 8601 format, but
        /// without the timezone designator.
        /// </summary>
        public string GreaterEqual { get; set; }

        /// <summary>
        /// A date-time value that can be used in search criteria to filter results to only include items
        /// with a date-time lower to the specified value (before). The date-time is in ISO 8601 format, but
        /// without the timezone designator.
        /// </summary>
        public string Lower { get; set; }

        /// <summary>
        /// A date-time value that can be used in search criteria to filter results to only include items
        /// with a date-time lower than or equal to the specified value (equal or before). The date-time is in ISO 8601 format, but
        /// without the timezone designator.
        /// </summary>
        public string LowerEqual { get; set; }
    }
}
