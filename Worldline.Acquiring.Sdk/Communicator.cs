using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Authentication;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.Json;
using Worldline.Acquiring.Sdk.Logging;

namespace Worldline.Acquiring.Sdk
{
    /// <summary>
    /// Used to communicate with the Worldline Acquiring platform web services.
    /// </summary>
    /// <remarks>
    /// It contains all the logic to transform a request object to a HTTP request and a HTTP response to a response object.
    /// It is also thread safe.
    /// </remarks>
    public class Communicator : IDisposable, ILoggingCapable, IObfuscationCapable
    {
        /// <summary>
        /// Gets the <see cref="IMarshaller"/> object associated with this communicator. Never <c>null</c>.
        /// </summary>
        public IMarshaller Marshaller { get; }

        public Communicator(Uri apiEndpoint, IConnection connection, IAuthenticator authenticator, MetadataProvider metadataProvider, IMarshaller marshaller)
        {
            if (apiEndpoint == null)
            {
                throw new ArgumentException("apiEndpoint is required");
            }
            if (apiEndpoint.HasPath())
            {
                throw new ArgumentException("apiEndpoint should not contain a path");
            }
            if (apiEndpoint.HasUserInfoOrQueryOrFragment())
            {
                throw new ArgumentException("apiEndpoint should not contain user info, query or fragment");
            }
            ApiEndpoint = apiEndpoint;
            Connection = connection ?? throw new ArgumentException("connection is required");
            Authenticator = authenticator ?? throw new ArgumentException("authenticator is required");
            MetadataProvider = metadataProvider ?? throw new ArgumentException("metadataProvider is required");
            Marshaller = marshaller ?? throw new ArgumentException("marshaller is required");
        }

        #region IDisposable implementation
        /// <summary>
        /// Releases all resource used by the <see cref="T:Worldline.Acquiring.Sdk.Communicator"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the
        /// <see cref="T:Worldline.Acquiring.Sdk.Communicator"/>. The <see cref="Dispose"/> method leaves the
        /// <see cref="T:Worldline.Acquiring.Sdk.Communicator"/> in an unusable state. After calling <see cref="Dispose"/>,
        /// you must release all references to the <see cref="T:Worldline.Acquiring.Sdk.Communicator"/> so the garbage
        /// collector can reclaim the memory that the <see cref="T:Worldline.Acquiring.Sdk.Communicator"/> was occupying.</remarks>
        public void Dispose()
        {
            Connection.Dispose();
        }
        #endregion

        #region IObfuscationCapable implementation
        public BodyObfuscator BodyObfuscator
        {
            set => Connection.BodyObfuscator = value;
        }

        public HeaderObfuscator HeaderObfuscator
        {
            set => Connection.HeaderObfuscator = value;
        }
        #endregion

        #region ILoggingCapable implementation
        public void EnableLogging(ICommunicatorLogger communicatorLogger)
        {
            Connection.EnableLogging(communicatorLogger);
        }

        public void DisableLogging()
        {
            Connection.DisableLogging();
        }
        #endregion

        #region HTTP methods
        /// <summary>
        /// Corresponds to the HTTP Get method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="context">The optional call context to use</param>
        /// <typeparam name="T">Type of the response.</typeparam>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        [Obsolete("Use the overload that takes a CancellationToken instead. This method will be removed in a future major version.")]
        public async Task<T> Get<T>(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                                    CallContext context)
        {
            return await Get<T>(relativePath, requestHeaders, requestParameters, context, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Corresponds to the HTTP Get method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="context">The optional call context to use</param>
        /// <param name="cancellationToken">A token to cancel the call.</param>
        /// <typeparam name="T">Type of the response.</typeparam>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        public async Task<T> Get<T>(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                                    CallContext context, CancellationToken cancellationToken)
        {
            var requestParameterList = requestParameters?.ToRequestParameters();
            var uri = ToAbsoluteUri(relativePath, requestParameterList);
            requestHeaders = requestHeaders ?? new List<IRequestHeader>();
            requestHeaders = await AddGenericHeaders(HttpMethod.Get, uri, requestHeaders, context, cancellationToken).ConfigureAwait(false);
            return await Send<T>(HttpMethod.Get, uri, requestHeaders, null, null, relativePath, context, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Corresponds to the HTTP Get method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="bodyHandler">A callback that receives the contents of the body as a stream</param>
        /// <param name="context">The optional call context to use</param>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        /// <exception cref="BodyHandlerException">when the BodyHandler throws an exception</exception>
        [Obsolete("Use the overload that takes a CancellationToken and an asynchronous body handler instead. This method will be removed in a future major version.")]
        public async Task Get(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                              Action<Stream, IEnumerable<IResponseHeader>> bodyHandler, CallContext context)
        {
            await Get(relativePath, requestHeaders, requestParameters, ToAsyncBodyHandler(bodyHandler), context, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Corresponds to the HTTP Get method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="bodyHandler">An asynchronous callback that receives the contents of the body as a stream. The response stays open until it has completed.</param>
        /// <param name="context">The optional call context to use</param>
        /// <param name="cancellationToken">A token to cancel the call.</param>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        /// <exception cref="BodyHandlerException">when the BodyHandler throws an exception</exception>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        public async Task Get(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                              Func<Stream, IEnumerable<IResponseHeader>, Task> bodyHandler, CallContext context, CancellationToken cancellationToken)
        {
            var requestParameterList = requestParameters?.ToRequestParameters();
            var uri = ToAbsoluteUri(relativePath, requestParameterList);
            requestHeaders = requestHeaders ?? new List<IRequestHeader>();
            requestHeaders = await AddGenericHeaders(HttpMethod.Get, uri, requestHeaders, context, cancellationToken).ConfigureAwait(false);
            await SendWithBodyHandler(HttpMethod.Get, uri, requestHeaders, null, null, relativePath, context, bodyHandler, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Corresponds to the HTTP DELETE method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="context">The optional call context to use</param>
        /// <typeparam name="T">Type of the response.</typeparam>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        [Obsolete("Use the overload that takes a CancellationToken instead. This method will be removed in a future major version.")]
        public async Task<T> Delete<T>(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                                       CallContext context)
        {
            return await Delete<T>(relativePath, requestHeaders, requestParameters, context, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Corresponds to the HTTP DELETE method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="context">The optional call context to use</param>
        /// <param name="cancellationToken">A token to cancel the call.</param>
        /// <typeparam name="T">Type of the response.</typeparam>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        public async Task<T> Delete<T>(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                                       CallContext context, CancellationToken cancellationToken)
        {
            var requestParameterList = requestParameters?.ToRequestParameters();
            var uri = ToAbsoluteUri(relativePath, requestParameterList);
            requestHeaders = requestHeaders ?? new List<IRequestHeader>();
            requestHeaders = await AddGenericHeaders(HttpMethod.Delete, uri, requestHeaders, context, cancellationToken).ConfigureAwait(false);
            return await Send<T>(HttpMethod.Delete, uri, requestHeaders, null, null, relativePath, context, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Corresponds to the HTTP DELETE method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="bodyHandler">A callback that receives the contents of the body as a stream</param>
        /// <param name="context">The optional call context to use</param>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        /// <exception cref="BodyHandlerException">when the BodyHandler throws an exception</exception>
        [Obsolete("Use the overload that takes a CancellationToken and an asynchronous body handler instead. This method will be removed in a future major version.")]
        public async Task Delete(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                                 Action<Stream, IEnumerable<IResponseHeader>> bodyHandler, CallContext context)
        {
            await Delete(relativePath, requestHeaders, requestParameters, ToAsyncBodyHandler(bodyHandler), context, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Corresponds to the HTTP DELETE method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="bodyHandler">An asynchronous callback that receives the contents of the body as a stream. The response stays open until it has completed.</param>
        /// <param name="context">The optional call context to use</param>
        /// <param name="cancellationToken">A token to cancel the call.</param>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        /// <exception cref="BodyHandlerException">when the BodyHandler throws an exception</exception>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        public async Task Delete(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                                 Func<Stream, IEnumerable<IResponseHeader>, Task> bodyHandler, CallContext context, CancellationToken cancellationToken)
        {
            var requestParameterList = requestParameters?.ToRequestParameters();
            var uri = ToAbsoluteUri(relativePath, requestParameterList);
            requestHeaders = requestHeaders ?? new List<IRequestHeader>();
            requestHeaders = await AddGenericHeaders(HttpMethod.Delete, uri, requestHeaders, context, cancellationToken).ConfigureAwait(false);
            await SendWithBodyHandler(HttpMethod.Delete, uri, requestHeaders, null, null, relativePath, context, bodyHandler, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Corresponds to the HTTP POST method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="requestBody">The optional request body to send.</param>
        /// <param name="context">The optional call context to use</param>
        /// <typeparam name="T">Type of the response.</typeparam>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        [Obsolete("Use the overload that takes a CancellationToken instead. This method will be removed in a future major version.")]
        public async Task<T> Post<T>(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                                     object requestBody, CallContext context)
        {
            return await Post<T>(relativePath, requestHeaders, requestParameters, requestBody, context, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Corresponds to the HTTP POST method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="requestBody">The optional request body to send.</param>
        /// <param name="context">The optional call context to use</param>
        /// <param name="cancellationToken">A token to cancel the call.</param>
        /// <typeparam name="T">Type of the response.</typeparam>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        public async Task<T> Post<T>(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                                     object requestBody, CallContext context, CancellationToken cancellationToken)
        {
            switch (requestBody)
            {
                case MultipartFormDataObject multipartFormDataObject:
                    return await Post<T>(relativePath, requestHeaders, requestParameters, multipartFormDataObject, context, cancellationToken).ConfigureAwait(false);
                case IMultipartFormDataRequest multipartFormDataRequest:
                {
                    var multipart = multipartFormDataRequest.ToMultipartFormDataObject();
                    return await Post<T>(relativePath, requestHeaders, requestParameters, multipart, context, cancellationToken).ConfigureAwait(false);
                }
            }

            var requestParameterList = requestParameters?.ToRequestParameters();
            var uri = ToAbsoluteUri(relativePath, requestParameterList);
            requestHeaders = requestHeaders ?? new List<IRequestHeader>();

            string requestJson = null;
            var requestHeaderList = requestHeaders.ToList();
            if (requestBody != null)
            {
                requestHeaderList.Add(new EntityHeader("Content-Type", "application/json"));
                requestJson = Marshaller.Marshal(requestBody);
            }

            requestHeaders = await AddGenericHeaders(HttpMethod.Post, uri, requestHeaderList, context, cancellationToken).ConfigureAwait(false);
            return await Send<T>(HttpMethod.Post, uri, requestHeaders, requestJson, null, relativePath, context, cancellationToken).ConfigureAwait(false);
        }

        private async Task<T> Post<T>(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                                      MultipartFormDataObject multipart, CallContext context, CancellationToken cancellationToken)
        {
            var requestParameterList = requestParameters?.ToRequestParameters();
            var uri = ToAbsoluteUri(relativePath, requestParameterList);
            requestHeaders = requestHeaders ?? new List<IRequestHeader>();

            var requestHeaderList = requestHeaders.ToList();
            requestHeaderList.Add(new EntityHeader("Content-Type", multipart.ContentType));

            requestHeaders = await AddGenericHeaders(HttpMethod.Post, uri, requestHeaderList, context, cancellationToken).ConfigureAwait(false);
            return await Send<T>(HttpMethod.Post, uri, requestHeaders, null, multipart, relativePath, context, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Corresponds to the HTTP POST method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="requestBody">The optional request body to send.</param>
        /// <param name="bodyHandler">A callback that receives the contents of the body as a stream</param>
        /// <param name="context">The optional call context to use</param>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        /// <exception cref="BodyHandlerException">when the BodyHandler throws an exception</exception>
        [Obsolete("Use the overload that takes a CancellationToken and an asynchronous body handler instead. This method will be removed in a future major version.")]
        public async Task Post(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                               object requestBody, Action<Stream, IEnumerable<IResponseHeader>> bodyHandler, CallContext context)
        {
            await Post(relativePath, requestHeaders, requestParameters, requestBody, ToAsyncBodyHandler(bodyHandler), context, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Corresponds to the HTTP POST method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="requestBody">The optional request body to send.</param>
        /// <param name="bodyHandler">An asynchronous callback that receives the contents of the body as a stream. The response stays open until it has completed.</param>
        /// <param name="context">The optional call context to use</param>
        /// <param name="cancellationToken">A token to cancel the call.</param>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        /// <exception cref="BodyHandlerException">when the BodyHandler throws an exception</exception>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        public async Task Post(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                               object requestBody, Func<Stream, IEnumerable<IResponseHeader>, Task> bodyHandler, CallContext context, CancellationToken cancellationToken)
        {
            switch (requestBody)
            {
                case MultipartFormDataObject multipartFormDataObject:
                    await Post(relativePath, requestHeaders, requestParameters, multipartFormDataObject, bodyHandler, context, cancellationToken).ConfigureAwait(false);
                    return;
                case IMultipartFormDataRequest multipartFormDataRequest:
                {
                    var multipart = multipartFormDataRequest.ToMultipartFormDataObject();
                    await Post(relativePath, requestHeaders, requestParameters, multipart, bodyHandler, context, cancellationToken).ConfigureAwait(false);
                    return;
                }
            }

            var requestParameterList = requestParameters?.ToRequestParameters();
            var uri = ToAbsoluteUri(relativePath, requestParameterList);
            requestHeaders = requestHeaders ?? new List<IRequestHeader>();

            string requestJson = null;
            var requestHeaderList = requestHeaders.ToList();
            if (requestBody != null)
            {
                requestHeaderList.Add(new EntityHeader("Content-Type", "application/json"));
                requestJson = Marshaller.Marshal(requestBody);
            }

            requestHeaders = await AddGenericHeaders(HttpMethod.Post, uri, requestHeaderList, context, cancellationToken).ConfigureAwait(false);
            await SendWithBodyHandler(HttpMethod.Post, uri, requestHeaders, requestJson, null, relativePath, context, bodyHandler, cancellationToken).ConfigureAwait(false);
        }

        private async Task Post(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                                MultipartFormDataObject multipart, Func<Stream, IEnumerable<IResponseHeader>, Task> bodyHandler, CallContext context, CancellationToken cancellationToken)
        {
            var requestParameterList = requestParameters?.ToRequestParameters();
            var uri = ToAbsoluteUri(relativePath, requestParameterList);
            requestHeaders = requestHeaders ?? new List<IRequestHeader>();

            var requestHeaderList = requestHeaders.ToList();
            requestHeaderList.Add(new EntityHeader("Content-Type", multipart.ContentType));

            requestHeaders = await AddGenericHeaders(HttpMethod.Post, uri, requestHeaderList, context, cancellationToken).ConfigureAwait(false);
            await SendWithBodyHandler(HttpMethod.Post, uri, requestHeaders, null, multipart, relativePath, context, bodyHandler, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Corresponds to the HTTP PUT method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="requestBody">The optional request body to send.</param>
        /// <param name="context">The optional call context to use</param>
        /// <typeparam name="T">Type of the response.</typeparam>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        [Obsolete("Use the overload that takes a CancellationToken instead. This method will be removed in a future major version.")]
        public async Task<T> Put<T>(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                                    object requestBody, CallContext context)
        {
            return await Put<T>(relativePath, requestHeaders, requestParameters, requestBody, context, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Corresponds to the HTTP PUT method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="requestBody">The optional request body to send.</param>
        /// <param name="context">The optional call context to use</param>
        /// <param name="cancellationToken">A token to cancel the call.</param>
        /// <typeparam name="T">Type of the response.</typeparam>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        public async Task<T> Put<T>(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                                    object requestBody, CallContext context, CancellationToken cancellationToken)
        {
            switch (requestBody)
            {
                case MultipartFormDataObject multipartFormDataObject:
                    return await Put<T>(relativePath, requestHeaders, requestParameters, multipartFormDataObject, context, cancellationToken).ConfigureAwait(false);
                case IMultipartFormDataRequest multipartFormDataRequest:
                {
                    var multipart = multipartFormDataRequest.ToMultipartFormDataObject();
                    return await Put<T>(relativePath, requestHeaders, requestParameters, multipart, context, cancellationToken).ConfigureAwait(false);
                }
            }

            var requestParameterList = requestParameters?.ToRequestParameters();
            var uri = ToAbsoluteUri(relativePath, requestParameterList);
            requestHeaders = requestHeaders ?? new List<IRequestHeader>();

            string requestJson = null;
            var requestHeaderList = requestHeaders.ToList();
            if (requestBody != null)
            {
                requestHeaderList.Add(new EntityHeader("Content-Type", "application/json"));
                requestJson = Marshaller.Marshal(requestBody);
            }

            requestHeaders = await AddGenericHeaders(HttpMethod.Put, uri, requestHeaderList, context, cancellationToken).ConfigureAwait(false);
            return await Send<T>(HttpMethod.Put, uri, requestHeaders, requestJson, null, relativePath, context, cancellationToken).ConfigureAwait(false);
        }

        private async Task<T> Put<T>(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                                     MultipartFormDataObject multipart, CallContext context, CancellationToken cancellationToken)
        {
            var requestParameterList = requestParameters?.ToRequestParameters();
            var uri = ToAbsoluteUri(relativePath, requestParameterList);
            requestHeaders = requestHeaders ?? new List<IRequestHeader>();

            var requestHeaderList = requestHeaders.ToList();
            requestHeaderList.Add(new EntityHeader("Content-Type", multipart.ContentType));

            requestHeaders = await AddGenericHeaders(HttpMethod.Put, uri, requestHeaderList, context, cancellationToken).ConfigureAwait(false);
            return await Send<T>(HttpMethod.Put, uri, requestHeaders, null, multipart, relativePath, context, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Corresponds to the HTTP PUT method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="requestBody">The optional request body to send.</param>
        /// <param name="bodyHandler">A callback that receives the contents of the body as a stream</param>
        /// <param name="context">The optional call context to use</param>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        /// <exception cref="BodyHandlerException">when the BodyHandler throws an exception</exception>
        [Obsolete("Use the overload that takes a CancellationToken and an asynchronous body handler instead. This method will be removed in a future major version.")]
        public async Task Put(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                              object requestBody, Action<Stream, IEnumerable<IResponseHeader>> bodyHandler, CallContext context)
        {
            await Put(relativePath, requestHeaders, requestParameters, requestBody, ToAsyncBodyHandler(bodyHandler), context, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Corresponds to the HTTP PUT method.
        /// </summary>
        /// <param name="relativePath">The path to call, relative to the base URI.</param>
        /// <param name="requestHeaders">An optional list of request headers.</param>
        /// <param name="requestParameters">The optional set of request parameters.</param>
        /// <param name="requestBody">The optional request body to send.</param>
        /// <param name="bodyHandler">An asynchronous callback that receives the contents of the body as a stream. The response stays open until it has completed.</param>
        /// <param name="context">The optional call context to use</param>
        /// <param name="cancellationToken">A token to cancel the call.</param>
        /// <exception cref="CommunicationException"> when an exception occurred communicating with the Worldline Acquiring platform</exception>
        /// <exception cref="ResponseException">when an error response was received from the Worldline Acquiring platform</exception>
        /// <exception cref="BodyHandlerException">when the BodyHandler throws an exception</exception>
        /// <exception cref="OperationCanceledException">when the cancellation token was cancelled</exception>
        public async Task Put(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                              object requestBody, Func<Stream, IEnumerable<IResponseHeader>, Task> bodyHandler, CallContext context, CancellationToken cancellationToken)
        {
            switch (requestBody)
            {
                case MultipartFormDataObject multipartFormDataObject:
                    await Put(relativePath, requestHeaders, requestParameters, multipartFormDataObject, bodyHandler, context, cancellationToken).ConfigureAwait(false);
                    return;
                case IMultipartFormDataRequest multipartFormDataRequest:
                {
                    var multipart = multipartFormDataRequest.ToMultipartFormDataObject();
                    await Put(relativePath, requestHeaders, requestParameters, multipart, bodyHandler, context, cancellationToken).ConfigureAwait(false);
                    return;
                }
            }

            var requestParameterList = requestParameters?.ToRequestParameters();
            var uri = ToAbsoluteUri(relativePath, requestParameterList);
            requestHeaders = requestHeaders ?? new List<IRequestHeader>();

            string requestJson = null;
            var requestHeaderList = requestHeaders.ToList();
            if (requestBody != null)
            {
                requestHeaderList.Add(new EntityHeader("Content-Type", "application/json"));
                requestJson = Marshaller.Marshal(requestBody);
            }

            requestHeaders = await AddGenericHeaders(HttpMethod.Put, uri, requestHeaderList, context, cancellationToken).ConfigureAwait(false);
            await SendWithBodyHandler(HttpMethod.Put, uri, requestHeaders, requestJson, null, relativePath, context, bodyHandler, cancellationToken).ConfigureAwait(false);
        }

        private async Task Put(string relativePath, IEnumerable<IRequestHeader> requestHeaders, AbstractParamRequest requestParameters,
                               MultipartFormDataObject multipart, Func<Stream, IEnumerable<IResponseHeader>, Task> bodyHandler, CallContext context, CancellationToken cancellationToken)
        {
            var requestParameterList = requestParameters?.ToRequestParameters();
            var uri = ToAbsoluteUri(relativePath, requestParameterList);
            requestHeaders = requestHeaders ?? new List<IRequestHeader>();

            var requestHeaderList = requestHeaders.ToList();
            requestHeaderList.Add(new EntityHeader("Content-Type", multipart.ContentType));

            requestHeaders = await AddGenericHeaders(HttpMethod.Put, uri, requestHeaderList, context, cancellationToken).ConfigureAwait(false);
            await SendWithBodyHandler(HttpMethod.Put, uri, requestHeaders, null, multipart, relativePath, context, bodyHandler, cancellationToken).ConfigureAwait(false);
        }

        // Sends the request through the connection. A connection that implements ICancellableConnection gets the token and an asynchronous response handler.
        // Any other connection is used as it always was, and the token is only honoured before the request is sent.
        private async Task<T> Send<T>(HttpMethod httpMethod, Uri uri, IEnumerable<IRequestHeader> requestHeaders, string body, MultipartFormDataObject multipart,
                                      string relativePath, CallContext context, CancellationToken cancellationToken)
        {
            if (Connection is ICancellableConnection cancellableConnection)
            {
                return await SendCancellable(cancellableConnection, httpMethod, uri, requestHeaders, body, multipart,
                    (status, stream, headers) => ProcessResponseAsync<T>(status, stream, headers, relativePath, context, null, cancellationToken),
                    cancellationToken).ConfigureAwait(false);
            }

            cancellationToken.ThrowIfCancellationRequested();
            return await SendPlain(Connection, httpMethod, uri, requestHeaders, body, multipart,
                (status, stream, headers) => ProcessResponse<T>(status, stream, headers, relativePath, context)).ConfigureAwait(false);
        }

        private async Task SendWithBodyHandler(HttpMethod httpMethod, Uri uri, IEnumerable<IRequestHeader> requestHeaders, string body, MultipartFormDataObject multipart,
                                               string relativePath, CallContext context, Func<Stream, IEnumerable<IResponseHeader>, Task> bodyHandler, CancellationToken cancellationToken)
        {
            if (bodyHandler == null)
            {
                // without a body handler the response body is unmarshalled, like it always was
                await Send<object>(httpMethod, uri, requestHeaders, body, multipart, relativePath, context, cancellationToken).ConfigureAwait(false);
                return;
            }

            if (Connection is ICancellableConnection cancellableConnection)
            {
                await SendCancellable<object>(cancellableConnection, httpMethod, uri, requestHeaders, body, multipart,
                    (status, stream, headers) => ProcessResponseAsync<object>(status, stream, headers, relativePath, context, bodyHandler, cancellationToken),
                    cancellationToken).ConfigureAwait(false);
                return;
            }

            cancellationToken.ThrowIfCancellationRequested();

            // A connection that does not implement ICancellableConnection can only call a synchronous response handler, and cannot wait for an asynchronous one.
            // So it checks the response and reads the body into memory, and the asynchronous body handler is called after the connection has returned.
            Stream capturedBody = null;
            IEnumerable<IResponseHeader> capturedHeaders = null;
            await SendPlain<object>(Connection, httpMethod, uri, requestHeaders, body, multipart,
                (status, stream, headers) => ProcessResponse<object>(status, stream, headers, relativePath, context, (bodyStream, bodyHeaders) =>
                {
                    var copy = new MemoryStream();
                    bodyStream.CopyTo(copy);
                    copy.Position = 0;
                    capturedBody = copy;
                    capturedHeaders = bodyHeaders.ToList();
                })).ConfigureAwait(false);

            // The token is deliberately not checked here: the response has already arrived, and the body handler can still honour the token itself.
            using (capturedBody)
            {
                await HandleBody(bodyHandler, capturedBody, capturedHeaders, cancellationToken).ConfigureAwait(false);
            }
        }

        private static Task<T> SendCancellable<T>(ICancellableConnection connection, HttpMethod httpMethod, Uri uri, IEnumerable<IRequestHeader> requestHeaders, string body, MultipartFormDataObject multipart,
                                                  Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, Task<T>> responseHandler, CancellationToken cancellationToken)
        {
            if (httpMethod == HttpMethod.Get)
            {
                return connection.Get(uri, requestHeaders, responseHandler, cancellationToken);
            }
            if (httpMethod == HttpMethod.Delete)
            {
                return connection.Delete(uri, requestHeaders, responseHandler, cancellationToken);
            }
            if (httpMethod == HttpMethod.Post)
            {
                return multipart != null
                    ? connection.Post(uri, requestHeaders, multipart, responseHandler, cancellationToken)
                    : connection.Post(uri, requestHeaders, body, responseHandler, cancellationToken);
            }
            if (httpMethod == HttpMethod.Put)
            {
                return multipart != null
                    ? connection.Put(uri, requestHeaders, multipart, responseHandler, cancellationToken)
                    : connection.Put(uri, requestHeaders, body, responseHandler, cancellationToken);
            }
            throw new ArgumentException("Unsupported HTTP method " + httpMethod);
        }

        private static Task<T> SendPlain<T>(IConnection connection, HttpMethod httpMethod, Uri uri, IEnumerable<IRequestHeader> requestHeaders, string body, MultipartFormDataObject multipart,
                                            Func<HttpStatusCode, Stream, IEnumerable<IResponseHeader>, T> responseHandler)
        {
            if (httpMethod == HttpMethod.Get)
            {
                return connection.Get(uri, requestHeaders, responseHandler);
            }
            if (httpMethod == HttpMethod.Delete)
            {
                return connection.Delete(uri, requestHeaders, responseHandler);
            }
            if (httpMethod == HttpMethod.Post)
            {
                return multipart != null
                    ? connection.Post(uri, requestHeaders, multipart, responseHandler)
                    : connection.Post(uri, requestHeaders, body, responseHandler);
            }
            if (httpMethod == HttpMethod.Put)
            {
                return multipart != null
                    ? connection.Put(uri, requestHeaders, multipart, responseHandler)
                    : connection.Put(uri, requestHeaders, body, responseHandler);
            }
            throw new ArgumentException("Unsupported HTTP method " + httpMethod);
        }

        private static Func<Stream, IEnumerable<IResponseHeader>, Task> ToAsyncBodyHandler(Action<Stream, IEnumerable<IResponseHeader>> bodyHandler)
        {
            if (bodyHandler == null)
            {
                return null;
            }
            return (stream, headers) =>
            {
                bodyHandler(stream, headers);
                return Task.CompletedTask;
            };
        }
        #endregion

        /// <summary>
        /// Utility method that delegates the call to this communicator's connection if that's an instance of <see cref="IPooledConnection"/>.
        /// If not this method does nothing.
        /// <seealso cref="IPooledConnection.CloseExpiredConnections"/>
        /// </summary>
        public void CloseExpiredConnections()
        {
            if (Connection is IPooledConnection pooledConnection)
            {
                pooledConnection.CloseExpiredConnections();
            }
        }

        /// <summary>
        /// Utility method that delegates the call to this communicator's connection if that's an instance of
        /// <see cref="IPooledConnection"/>.
        /// </summary>
        /// <param name="timespan">Idle time.</param>
        public void CloseIdleConnections(TimeSpan timespan)
        {
            if (Connection is IPooledConnection pooledConnection)
            {
                pooledConnection.CloseIdleConnections(timespan);
            }
        }

        internal Uri ApiEndpoint { get; }

        internal IConnection Connection { get; }

        internal MetadataProvider MetadataProvider { get; }

        internal IAuthenticator Authenticator { get; }

        internal Uri ToAbsoluteUri(string relativePath, IEnumerable<RequestParam> requestParameters)
        {
            string absolutePath;
            if (relativePath.StartsWith("/", StringComparison.Ordinal))
            {
                absolutePath = relativePath;
            }
            else
            {
                absolutePath = "/" + relativePath;
            }

            var uriBuilder = new UriBuilder
            {
                Scheme = ApiEndpoint.Scheme,
                Host = ApiEndpoint.Host,
                Port = ApiEndpoint.Port,
                Path = absolutePath
            };

            if (requestParameters != null)
            {
                foreach (var nvp in requestParameters)
                {
                    uriBuilder.AddParameter(nvp.Name, nvp.Value);
                }
            }

            return uriBuilder.Uri;
        }

        /// <summary>
        /// Adds the necessary headers to the given list of headers. This includes the authorization header,
        /// which uses other headers, so when you need to override this method,
        /// make sure to call <c>base.AddGenericHeaders</c> at the <i>end</i> of your overridden method.
        /// </summary>
        protected async Task<IEnumerable<IRequestHeader>> AddGenericHeaders(HttpMethod httpMethod, Uri uri, IEnumerable<IRequestHeader> requestHeaders, CallContext context)
        {
            return await AddGenericHeaders(httpMethod, uri, requestHeaders, context, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds the necessary headers to the given list of headers, like <see cref="AddGenericHeaders(HttpMethod, Uri, IEnumerable{IRequestHeader}, CallContext)"/>,
        /// but passes the cancellation token on to the authenticator if that is an <see cref="ICancellableAuthenticator"/>.
        /// Otherwise the token is only honoured before the authenticator is called.
        /// </summary>
        protected async Task<IEnumerable<IRequestHeader>> AddGenericHeaders(HttpMethod httpMethod, Uri uri, IEnumerable<IRequestHeader> requestHeaders, CallContext context, CancellationToken cancellationToken)
        {
            var requestHeaderList = requestHeaders.ToList();

            // add server meta info headers
            requestHeaderList.AddRange(MetadataProvider.ServerMetadataHeaders);

            // add date header
            requestHeaderList.Add(new RequestHeader("Date", GetHeaderDateString()));

            // add trace id ehader
            requestHeaderList.Add(new RequestHeader("Trace-Id", Guid.NewGuid().ToString()));

            // no context specific headers at this time

            // add authorization
            string authorization;
            if (Authenticator is ICancellableAuthenticator cancellableAuthenticator)
            {
                authorization = await cancellableAuthenticator.GetAuthorization(httpMethod, uri, requestHeaderList, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                cancellationToken.ThrowIfCancellationRequested();
                authorization = await Authenticator.GetAuthorization(httpMethod, uri, requestHeaderList);
            }
            requestHeaderList.Add(new RequestHeader("Authorization", authorization));
            return requestHeaderList;
        }

        /// <summary>
        /// Gets the date in the preferred format for the HTTP date header (RFC1123).
        /// </summary>
        /// <returns>The header date string.</returns>
        protected static string GetHeaderDateString()
        {
            return DateTime.UtcNow.ToString("r");
        }

        /// <summary>
        /// Checks the response for errors and throws an exception if necessary.
        /// </summary>
        protected static void ThrowExceptionIfNecessary(HttpStatusCode statusCode, Stream stream, IEnumerable<IResponseHeader> headers, string requestPath)
        {
            var intStatusCode = (int)statusCode;
            // status codes in the 100 or 300 range are not expected
            if (intStatusCode < 200 || intStatusCode >= 300)
            {
                var sr = new StreamReader(stream);
                var body = sr.ReadToEnd();

                if (string.IsNullOrEmpty(body) || !IsJson(headers))
                {
                    var cause = new ResponseException(statusCode, body, headers);
                    if (intStatusCode == (int)HttpStatusCode.NotFound)
                    {
                        throw new NotFoundException("The requested resource was not found; invalid path: " + requestPath, cause);
                    }
                    throw new CommunicationException(cause);
                }
                throw new ResponseException(statusCode, body, headers);
            }
        }

        protected T ProcessResponse<T>(HttpStatusCode statusCode, Stream stream, IEnumerable<IResponseHeader> headers, string requestPath, CallContext context, Action<Stream, IEnumerable<IResponseHeader>> bodyHandler = null)
        {
            // no context specific headers at this time
            ThrowExceptionIfNecessary(statusCode, stream, headers, requestPath);
            if (bodyHandler == null) {
                return Marshaller.Unmarshal<T>(stream);
            }
            try
            {
                bodyHandler(stream, headers);
            }
            catch (Exception e)
            {
                throw new BodyHandlerException("The bodyhandler threw an exception", e);
            }
            return default;
        }

        /// <summary>
        /// Like <c>ProcessResponse</c>, but the body handler is asynchronous, so it can await while the response is still open.
        /// If the body handler is cancelled because the given token was cancelled, that cancellation is not wrapped in a <see cref="BodyHandlerException"/>.
        /// </summary>
        protected async Task<T> ProcessResponseAsync<T>(HttpStatusCode statusCode, Stream stream, IEnumerable<IResponseHeader> headers, string requestPath, CallContext context, Func<Stream, IEnumerable<IResponseHeader>, Task> bodyHandler, CancellationToken cancellationToken)
        {
            // no context specific headers at this time
            ThrowExceptionIfNecessary(statusCode, stream, headers, requestPath);
            if (bodyHandler == null) {
                return Marshaller.Unmarshal<T>(stream);
            }
            await HandleBody(bodyHandler, stream, headers, cancellationToken).ConfigureAwait(false);
            return default;
        }

        private static async Task HandleBody(Func<Stream, IEnumerable<IResponseHeader>, Task> bodyHandler, Stream stream, IEnumerable<IResponseHeader> headers, CancellationToken cancellationToken)
        {
            try
            {
                await bodyHandler(stream, headers).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new BodyHandlerException("The bodyhandler threw an exception", e);
            }
        }

        private static bool IsJson(IEnumerable<IResponseHeader> headers)
        {
            var contentType = headers?.GetHeaderValue("Content-Type")?.ToLower();
            return contentType == null
                   || "application/json".Equals(contentType) || contentType.StartsWith("application/json", StringComparison.Ordinal)
                   || "application/problem+json".Equals(contentType) || contentType.StartsWith("application/problem+json", StringComparison.Ordinal);
        }
    }
}
