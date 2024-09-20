/*
 * This file was automatically generated.
 */
using System.Net;

namespace Worldline.Acquiring.Sdk.V1
{
    /// <summary>
    /// Represents an error response from the Worldline Acquiring platform when validation of requests failed.
    /// </summary>
    public class ValidationException : ApiException
    {
        public ValidationException(HttpStatusCode statusCode, string responseBody, string type, string title, int? status, string detail, string instance)
            : base("The Worldline Acquiring platform returned an incorrect request error response", statusCode, responseBody, type, title, status, detail, instance)
        {

        }

        public ValidationException(string message, HttpStatusCode statusCode, string responseBody, string type, string title, int? status, string detail, string instance)
            : base(message, statusCode, responseBody, type, title, status, detail, instance)
        {

        }
    }
}
