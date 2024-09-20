/*
 * This file was automatically generated.
 */
using System;
using System.Net;
using Worldline.Acquiring.Sdk.V1.Domain;

namespace Worldline.Acquiring.Sdk.V1
{
    /// <summary>
    /// Factory for exceptions thrown by Worldline Acquiring platform API v1 resources.
    /// </summary>
    public static class ExceptionFactory
    {
        public static Exception CreateException(HttpStatusCode statusCode, string responseBody, object errorObject, CallContext context)
        {
            if (errorObject is ApiPaymentErrorResponse apiPaymentErrorResponse)
            {
                return CreateException(statusCode, responseBody, apiPaymentErrorResponse.Type, apiPaymentErrorResponse.Title, apiPaymentErrorResponse.Status, apiPaymentErrorResponse.Detail, apiPaymentErrorResponse.Instance, context);
            }
            if (errorObject == null)
            {
                return CreateException(statusCode, responseBody, null, null, null, null, null, context);
            }
            throw new ArgumentException("unsupported error object type: " + errorObject.GetType());
        }

        private static Exception CreateException(HttpStatusCode statusCode, string responseBody, string type, string title, int? status, string detail, string instance, CallContext context)
        {
            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:
                    return new ValidationException(statusCode, responseBody, type, title, status, detail, instance);
                case HttpStatusCode.Forbidden:
                    return new AuthorizationException(statusCode, responseBody, type, title, status, detail, instance);
                case HttpStatusCode.NotFound:
                    return new ReferenceException(statusCode, responseBody, type, title, status, detail, instance);
                case HttpStatusCode.Conflict:
                    return new ReferenceException(statusCode, responseBody, type, title, status, detail, instance);
                case HttpStatusCode.Gone:
                    return new ReferenceException(statusCode, responseBody, type, title, status, detail, instance);
                case HttpStatusCode.InternalServerError:
                    return new PlatformException(statusCode, responseBody, type, title, status, detail, instance);
                case HttpStatusCode.BadGateway:
                    return new PlatformException(statusCode, responseBody, type, title, status, detail, instance);
                case HttpStatusCode.ServiceUnavailable:
                    return new PlatformException(statusCode, responseBody, type, title, status, detail, instance);
                default:
                    return new ApiException(statusCode, responseBody, type, title, status, detail, instance);
            }
        }
    }
}
