/*
 * This file was automatically generated.
 */
using System.Net;

namespace Worldline.Acquiring.Sdk.V1
{
    /// <summary>
    /// Represents an error response from the Worldline Acquiring platform when API authorization failed.
    /// </summary>
    public class AuthorizationException : ApiException
    {
        public AuthorizationException(HttpStatusCode statusCode, string responseBody, string type, string title, int? status, string detail, string instance)
            : base("The Worldline Acquiring platform returned an API authorization error response", statusCode, responseBody, type, title, status, detail, instance)
        {

        }

        public AuthorizationException(string message, HttpStatusCode statusCode, string responseBody, string type, string title, int? status, string detail, string instance)
            : base(message, statusCode, responseBody, type, title, status, detail, instance)
        {

        }
    }
}
