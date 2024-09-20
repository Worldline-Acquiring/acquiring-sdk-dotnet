using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Worldline.Acquiring.Sdk.Authentication;

namespace Worldline.Acquiring.Sdk.Communication
{
    internal class TestAuthenticator : IAuthenticator
    {
        public Task<string> GetAuthorization(HttpMethod httpMethod, Uri resourceUri, IEnumerable<IRequestHeader> requestHeaders)
        {
            return Task.FromResult("Test");
        }
    }
}
