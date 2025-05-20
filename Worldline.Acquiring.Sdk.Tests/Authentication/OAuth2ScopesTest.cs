using System.Collections.Frozen;
using System.Collections.Immutable;
using NUnit.Framework;

namespace Worldline.Acquiring.Sdk.Authentication
{
    [TestFixture]
    public class OAuth2ScopesTest
    {
        [TestCase]
        public void TestAll()
        {
            var allScopes = OAuth2Scopes.All.ToFrozenSet();
            Assert.Contains("processing_payment", allScopes);
            Assert.Contains("processing_dcc_rate", allScopes);
            Assert.Contains("services_ping", allScopes);

            var allScopesString = string.Join(" ", allScopes);
            Assert.LessOrEqual(allScopesString.Length, 260, allScopesString + " is too long");
        }

        [TestCase]
        public void TestForV1()
        {
            var allScopes = OAuth2Scopes.ForApiVersion("v1").ToFrozenSet();
            Assert.Contains("processing_payment", allScopes);
            Assert.Contains("processing_dcc_rate", allScopes);
            Assert.Contains("services_ping", allScopes);
        }

        [TestCase]
        public void TestForUnknownApiVersion()
        {
            var scopes = OAuth2Scopes.ForApiVersion("v-1");
            Assert.IsEmpty(scopes);
        }

        [TestCase]
        public void TestForV1ProcessPayment()
        {
            var scopes = OAuth2Scopes.ForOperation("v1", "processPayment").ToFrozenSet();
            Assert.Contains("processing_payment", scopes);
        }

        [TestCase]
        public void TestForV1RequestDccRate()
        {
            var scopes = OAuth2Scopes.ForOperation("v1", "requestDccRate").ToFrozenSet();
            Assert.Contains("processing_dcc_rate", scopes);
        }

        [TestCase]
        public void TestForUnknownOperation()
        {
            var scopes = OAuth2Scopes.ForOperation("v1", "unknown");
            Assert.IsEmpty(scopes);
        }

        [Test]
        public void TestForOperationOfUnknownApiVersion()
        {
            var scopes = OAuth2Scopes.ForOperation("v-1", "processPayment");
            Assert.IsEmpty(scopes);
        }

        [TestCase]
        public void TestForV1Operations()
        {
            var scopes = OAuth2Scopes.ForOperations("v1", "processPayment", "requestDccRate", "unknown").ToFrozenSet();
            Assert.Contains("processing_payment", scopes);
            Assert.Contains("processing_dcc_rate", scopes);
            Assert.That(scopes, Has.None.EqualTo("services_ping"));
        }

        [Test]
        public void TestForOperationsOfUnknownApiVersion()
        {
            var scopes = OAuth2Scopes.ForOperations("v-1", "processPayment", "requestDccRate");
            Assert.IsEmpty(scopes);
        }

        [Test]
        public void TestForOperationsWithFilter()
        {
            var operationIds = ImmutableHashSet.Create("processPayment", "requestDccRate", "unknown");
            var scopes = OAuth2Scopes.ForOperations((v, o) => v == "v1" && operationIds.Contains(o)).ToFrozenSet();
            Assert.Contains("processing_payment", scopes);
            Assert.Contains("processing_dcc_rate", scopes);
            Assert.That(scopes, Has.None.EqualTo("services_ping"));
        }
    }
}
