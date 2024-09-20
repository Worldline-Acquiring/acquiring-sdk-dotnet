/*
 * This file was automatically generated.
 */
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace Worldline.Acquiring.Sdk.V1
{
    /// <summary>
    /// Represents an error response from the Worldline Acquiring platform.
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// Gets the HTTP status code that was returned by the Worldline Acquiring platform.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the raw response body that was returned by the Worldline Acquiring platform.
        /// </summary>
        public string ResponseBody { get; }

        /// <summary>
        /// Gets the <c>type</c> received from the Worldline Acquiring platform if available.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the <c>title</c> received from the Worldline Acquiring platform if available.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the <c>status</c> received from the Worldline Acquiring platform if available.
        /// </summary>
        public int? Status { get; }

        /// <summary>
        /// Gets the <c>detail</c> received from the Worldline Acquiring platform if available.
        /// </summary>
        public string Detail { get; }

        /// <summary>
        /// Gets the <c>instance</c> received from the Worldline Acquiring platform if available.
        /// </summary>
        public string Instance { get; }

        public ApiException(HttpStatusCode statusCode, string responseBody, string type, string title, int? status, string detail, string instance)
            : this("The Worldline Acquiring platform returned an error response", statusCode, responseBody, type, title, status, detail, instance)
        {

        }

        public ApiException(string message, HttpStatusCode statusCode, string responseBody, string type, string title, int? status, string detail, string instance)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
            Type = type;
            Title = title;
            Status = status;
            Detail = detail;
            Instance = instance;
        }

        public override string ToString()
        {
            var list = new StringBuilder(base.ToString());
            if (StatusCode > 0)
            {
                list.Append("; statusCode=").Append(StatusCode.ToString());
            }
            if (ResponseBody != null && ResponseBody.Any())
            {
                list.Append("; responseBody='").Append(ResponseBody).Append("'");
            }
            return list.ToString();
        }
    }
}
