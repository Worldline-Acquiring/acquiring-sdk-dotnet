/*
 * This file was automatically generated.
 */
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.Domain;

namespace Worldline.Acquiring.Sdk.V1.Disputedocuments
{
    /// <summary>
    /// Multipart/form-data parameters for
    /// <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Dispute-Documents/operation/uploadDisputeDocument">Upload Dispute Document</a>
    /// </summary>
    public class UploadDisputeDocumentRequest : IMultipartFormDataRequest
    {
        /// <summary>
        /// The file to upload as evidence. The file must be provided in the multipart form data of the request.
        /// </summary>
        public UploadableFile File { get; set; }

        public MultipartFormDataObject ToMultipartFormDataObject()
        {
            var result = new MultipartFormDataObject();
            if (File != null)
            {
                result.AddFile("file", File);
            }
            return result;
        }
    }
}
