using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using NUnit.Framework;
using Worldline.Acquiring.Sdk.Authentication;

namespace Worldline.Acquiring.Sdk.Communication
{
    /// <summary>
    /// <see cref="IConnection"/> and <see cref="IAuthenticator"/> are public, and implemented by users of the SDK, so they must not change.
    /// Cancellation support is added through new interfaces that extend them.
    /// </summary>
    [TestFixture]
    public class CancellableInterfacesTest
    {
        private const BindingFlags DeclaredMethods = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        [Test]
        public void TestIConnectionIsUnchanged()
        {
            var methods = typeof(IConnection).GetMethods(DeclaredMethods);

            Assert.That(methods.Select(method => method.Name), Is.EquivalentTo(new[] { "Get", "Delete", "Post", "Post", "Put", "Put" }));
            Assert.That(methods.SelectMany(method => method.GetParameters()).Select(parameter => parameter.ParameterType), Does.Not.Contain(typeof(CancellationToken)));
            Assert.That(typeof(IConnection).GetProperties(DeclaredMethods), Is.Empty);
        }

        [Test]
        public void TestIAuthenticatorIsUnchanged()
        {
            var methods = typeof(IAuthenticator).GetMethods(DeclaredMethods);

            Assert.That(methods.Select(method => method.Name), Is.EqualTo(new[] { "GetAuthorization" }));
            Assert.That(methods.Single().GetParameters().Length, Is.EqualTo(3));
            Assert.That(methods.Single().GetParameters().Select(parameter => parameter.ParameterType), Does.Not.Contain(typeof(CancellationToken)));
        }

        [Test]
        public void TestICancellableConnectionExtendsIConnectionWithTokenOverloads()
        {
            Assert.That(typeof(IConnection).IsAssignableFrom(typeof(ICancellableConnection)));

            var methods = typeof(ICancellableConnection).GetMethods(DeclaredMethods);

            Assert.That(methods.Select(method => method.Name), Is.EquivalentTo(new[] { "Get", "Delete", "Post", "Post", "Put", "Put" }));
            foreach (var method in methods)
            {
                Assert.That(method.GetParameters().Last().ParameterType, Is.EqualTo(typeof(CancellationToken)), method.ToString());
            }
        }

        [Test]
        public void TestICancellableAuthenticatorExtendsIAuthenticatorWithATokenOverload()
        {
            Assert.That(typeof(IAuthenticator).IsAssignableFrom(typeof(ICancellableAuthenticator)));

            var method = typeof(ICancellableAuthenticator).GetMethods(DeclaredMethods).Single();

            Assert.That(method.Name, Is.EqualTo("GetAuthorization"));
            Assert.That(method.GetParameters().Last().ParameterType, Is.EqualTo(typeof(CancellationToken)));
        }

        [Test]
        public void TestTheDefaultImplementationsSupportCancellation()
        {
            Assert.That(typeof(ICancellableConnection).IsAssignableFrom(typeof(DefaultConnection)));
            Assert.That(typeof(ICancellableAuthenticator).IsAssignableFrom(typeof(OAuth2Authenticator)));
        }
    }
}
