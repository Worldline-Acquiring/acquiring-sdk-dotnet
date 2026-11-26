using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.Util;

namespace Worldline.Acquiring.Sdk.Authentication
{
    /// <summary>
    /// Verifies how <see cref="OAuth2Authenticator"/> handles a <see cref="CancellationToken"/>, using a stub token endpoint on the loopback interface.
    /// </summary>
    [TestFixture]
    public class OAuth2AuthenticatorCancellationTest
    {
        private const string TokenResponse = "{\"access_token\":\"the-token\",\"token_type\":\"Bearer\",\"expires_in\":3600}";

        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(10);
        private static readonly Uri ResourceUri = new Uri("https://example.com/v1/resource");
        private static readonly List<IRequestHeader> NoHeaders = new List<IRequestHeader>();

        private static OAuth2Authenticator CreateAuthenticator(StubHttpServer server)
        {
            var configuration = new CommunicatorConfiguration()
                .WithOAuth2ClientId("client-id")
                .WithOAuth2ClientSecret("client-secret")
                .WithOAuth2TokenUri(new Uri(server.BaseUri, "token").ToString())
                .WithSocketTimeout(30000);
            return new OAuth2Authenticator(configuration);
        }

        private static async Task WaitFor(Task task)
        {
            var finished = await Task.WhenAny(task, Task.Delay(TestTimeout)).ConfigureAwait(false);
            Assert.That(finished, Is.SameAs(task), "timed out waiting for the test condition");
        }

        // A test that waits for a task that never completes would hang, so a task that takes too long fails the test instead
        private static async Task<T> WithTimeout<T>(Task<T> task)
        {
            var finished = await Task.WhenAny(task, Task.Delay(TestTimeout)).ConfigureAwait(false);
            if (finished != task)
            {
                throw new TimeoutException("the call did not complete after it was cancelled");
            }
            return await task.ConfigureAwait(false);
        }

        [Test]
        public async Task TestBothMethodsReturnTheAuthorizationAndShareTheToken()
        {
            using (var server = new StubHttpServer(TokenResponse))
            {
                var authenticator = CreateAuthenticator(server);

                // the existing method, which does not know about tokens
                Assert.That(await ((IAuthenticator)authenticator).GetAuthorization(HttpMethod.Get, ResourceUri, NoHeaders), Is.EqualTo("Bearer the-token"));
                using (var cts = new CancellationTokenSource())
                {
                    Assert.That(await authenticator.GetAuthorization(HttpMethod.Get, ResourceUri, NoHeaders, cts.Token), Is.EqualTo("Bearer the-token"));
                }

                // the access token is cached, so there was only one token request
                Assert.That(server.RequestCount, Is.EqualTo(1));
            }
        }

        [Test]
        public void TestAnAlreadyCancelledTokenRequestsNothing()
        {
            using (var server = new StubHttpServer(TokenResponse))
            using (var cts = new CancellationTokenSource())
            {
                var authenticator = CreateAuthenticator(server);
                cts.Cancel();

                var exception = Assert.CatchAsync<OperationCanceledException>(async () => await authenticator.GetAuthorization(HttpMethod.Get, ResourceUri, NoHeaders, cts.Token));

                Assert.That(exception, Is.Not.InstanceOf<CommunicationException>());
                Assert.That(server.RequestCount, Is.EqualTo(0));
            }
        }

        [Test]
        public async Task TestCancelDuringTheTokenRequestStopsTheCall()
        {
            // the stub accepts the token request but never answers it
            using (var server = new StubHttpServer(null))
            using (var cts = new CancellationTokenSource())
            {
                var authenticator = CreateAuthenticator(server);

                var call = authenticator.GetAuthorization(HttpMethod.Get, ResourceUri, NoHeaders, cts.Token);
                await WaitFor(server.FirstRequestReceived);
                Assert.That(call.IsCompleted, Is.False, "the token request must still be in flight");

                cts.Cancel();

                var exception = Assert.CatchAsync<OperationCanceledException>(async () => await WithTimeout(call));
                Assert.That(exception, Is.Not.InstanceOf<CommunicationException>());
            }
        }

        [Test]
        public async Task TestCancelWhileWaitingForAnotherCallsTokenRequestKeepsThatRequestAndReleasesTheLock()
        {
            using (var server = new StubHttpServer(null))
            using (var firstCts = new CancellationTokenSource())
            using (var secondCts = new CancellationTokenSource())
            {
                var authenticator = CreateAuthenticator(server);

                // the first call is busy with the token request, and holds the one lock for the token
                var first = authenticator.GetAuthorization(HttpMethod.Get, ResourceUri, NoHeaders, firstCts.Token);
                await WaitFor(server.FirstRequestReceived);

                // the second call has to wait for the first
                var second = authenticator.GetAuthorization(HttpMethod.Get, ResourceUri, NoHeaders, secondCts.Token);
                await Task.Delay(100);
                Assert.That(second.IsCompleted, Is.False, "the second call must wait for the first one");

                secondCts.Cancel();

                Assert.CatchAsync<OperationCanceledException>(async () => await WithTimeout(second));
                // cancelling the waiting call must not touch the call that is doing the work, and there was no second token request
                Assert.That(first.IsCompleted, Is.False);
                Assert.That(server.RequestCount, Is.EqualTo(1));

                // after the first call is cancelled too, the lock is free again: a new call can fetch a token, now that the endpoint answers
                firstCts.Cancel();
                Assert.CatchAsync<OperationCanceledException>(async () => await WithTimeout(first));
                server.ResponseBody = TokenResponse;
                using (var thirdCts = new CancellationTokenSource(TestTimeout))
                {
                    Assert.That(await authenticator.GetAuthorization(HttpMethod.Get, ResourceUri, NoHeaders, thirdCts.Token), Is.EqualTo("Bearer the-token"));
                }
            }
        }
    }
}
