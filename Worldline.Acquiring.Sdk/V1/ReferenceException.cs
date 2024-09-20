/*
 * This file was automatically generated.
 */
using System.Net;

namespace Worldline.Acquiring.Sdk.V1
{
    /// <summary>
    /// Represents an error response from the Worldline Acquiring platform when a non-existing or removed object is trying to be accessed.
    /// </summary>
    public class ReferenceException : ApiException
    {
        public ReferenceException(HttpStatusCode statusCode, string responseBody, string type, string title, int? status, string detail, string instance)
            : base("The Worldline Acquiring platform returned a reference error response", statusCode, responseBody, type, title, status, detail, instance)
        {

        }

        public ReferenceException(string message, HttpStatusCode statusCode, string responseBody, string type, string title, int? status, string detail, string instance)
            : base(message, statusCode, responseBody, type, title, status, detail, instance)
        {

        }
    }
}
