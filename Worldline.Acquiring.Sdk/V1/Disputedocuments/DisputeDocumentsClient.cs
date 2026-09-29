/*
 * This file was automatically generated.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.V1.Domain;

namespace Worldline.Acquiring.Sdk.V1.Disputedocuments
{
    /// <summary>
    /// DisputeDocuments client. Thread-safe.
    /// </summary>
    public class DisputeDocumentsClient : ApiResource
    {
        public DisputeDocumentsClient(ApiResource parent, IDictionary<string, string> pathContext) :
            base(parent, pathContext)
        {
        }

        /// <summary>
        /// Resource /dispute-management/v1/documents
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Dispute-Documents/operation/uploadDisputeDocument">Upload Dispute Document</a>
        /// </summary>
        /// <param name="body">UploadDisputeDocumentRequest</param>
        /// <param name="context">CallContext</param>
        /// <returns>UploadDocumentResponse</returns>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task<UploadDocumentResponse> UploadDisputeDocument(UploadDisputeDocumentRequest body, CallContext context = null)
        {
            var uri = InstantiateUri("/dispute-management/v1/documents", null);
            try
            {
                return await _communicator.Post<UploadDocumentResponse>(
                        uri,
                        null,
                        null,
                        body,
                        context).ConfigureAwait(false);
            }
            catch (ResponseException e)
            {
                object errorObject = _communicator.Marshaller.Unmarshal<ApiPaymentErrorResponse>(e.Body);
                throw ExceptionFactory.CreateException(e.StatusCode, e.Body, errorObject, context);
            }
        }

        /// <summary>
        /// Resource /dispute-management/v1/disputes/{disputeId}/documents/{documentId}
        /// - <a href="https://docs.acquiring.worldline-solutions.com/api-reference#tag/Dispute-Documents/operation/getDisputeDocument">Retrieve Dispute Document</a>
        /// </summary>
        /// <param name="disputeId">string</param>
        /// <param name="documentId">string</param>
        /// <param name="bodyHandler">A callback that receives the contents of the body as a stream</param>
        /// <param name="context">CallContext</param>
        /// <exception cref="ValidationException">if the request was not correct and couldn't be processed (HTTP status code 400)</exception>
        /// <exception cref="AuthorizationException">if the request was not allowed (HTTP status code 403)</exception>
        /// <exception cref="ReferenceException">if an object was attempted to be referenced that doesn't exist or has been removed,
        ///            or there was a conflict (HTTP status code 404, 409 or 410)</exception>
        /// <exception cref="PlatformException">if something went wrong at the Worldline Acquiring platform,
        ///            the Worldline Acquiring platform was unable to process a message from a downstream partner/acquirer,
        ///            or the service that you're trying to reach is temporary unavailable (HTTP status code 500, 502 or 503)</exception>
        /// <exception cref="ApiException">if the Worldline Acquiring platform returned any other error</exception>
        public async Task GetDisputeDocument(string disputeId, string documentId, Action<Stream, IEnumerable<IResponseHeader>> bodyHandler, CallContext context = null)
        {
            var pathContext = new Dictionary<string, string>
            {
                { "disputeId", disputeId },
                { "documentId", documentId }
            };
            var uri = InstantiateUri("/dispute-management/v1/disputes/{disputeId}/documents/{documentId}", pathContext);
            try
            {
                await _communicator.Get(
                        uri,
                        null,
                        null,
                        bodyHandler,
                        context).ConfigureAwait(false);
            }
            catch (ResponseException e)
            {
                object errorObject = _communicator.Marshaller.Unmarshal<ApiPaymentErrorResponse>(e.Body);
                throw ExceptionFactory.CreateException(e.StatusCode, e.Body, errorObject, context);
            }
        }
    }
}
