/*
 * This file was automatically generated.
 */
namespace Worldline.Acquiring.Sdk.V1.Domain
{
    public class DisputeDocument
    {
        /// <summary>
        /// The unique identifier of a document submitted as evidence for the dispute case, if applicable.
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        /// The name of the file submitted as evidence for the dispute case, if applicable.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The MIME type of the file submitted as evidence for the dispute case, if applicable.
        /// <p />
        /// Possible values are:
        /// <list type="bullet">
        ///   <item><description><c>application/pdf</c></description></item>
        ///   <item><description><c>image/jpeg</c></description></item>
        ///   <item><description><c>image/jpg</c></description></item>
        ///   <item><description><c>image/png</c></description></item>
        /// </list>
        /// </summary>
        public string MimeType { get; set; }
    }
}
