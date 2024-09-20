/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class ApiPaymentErrorResponse
    {
        /// <summary>
        /// Any relevant details about the error.<br />
        /// May include suggestions for handling it. Can be an empty string if no extra details are
        /// available.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// A URI reference that identifies the specific occurrence of the error.<br />
        /// It may or may not yield further information if dereferenced.
        /// </summary>
        public string Instance { get; set; }

        /// <summary>
        /// The HTTP status code of this error response.<br />
        /// Included to aid those frameworks that have a hard time working with anything other than
        /// the body of an HTTP response.
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// The human-readable version of the error.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The type of the error.<br />
        /// This is what you should match against when implementing error handling.<br />
        /// It is in the form of a URL that identifies the error type.
        /// </summary>
        public string Type { get; set; }
    }
}
