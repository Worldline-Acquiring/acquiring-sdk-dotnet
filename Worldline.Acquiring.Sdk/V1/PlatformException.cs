/*
 * This file was automatically generated.
 */
using System.Net;

namespace Worldline.Acquiring.Sdk.V1
{
    /// <summary>
    /// Represents an error response from the Worldline Acquiring platform when something went wrong at the Worldline Acquiring platform or further downstream.
    /// </summary>
    public class PlatformException : ApiException
    {
        public PlatformException(HttpStatusCode statusCode, string responseBody, string type, string title, int? status, string detail, string instance)
            : base("The Worldline Acquiring platform returned an error response", statusCode, responseBody, type, title, status, detail, instance)
        {

        }

        public PlatformException(string message, HttpStatusCode statusCode, string responseBody, string type, string title, int? status, string detail, string instance)
            : base(message, statusCode, responseBody, type, title, status, detail, instance)
        {

        }
    }
}
