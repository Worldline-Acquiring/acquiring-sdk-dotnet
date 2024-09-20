using NUnit.Framework;

namespace Worldline.Acquiring.Sdk
{
    [TestFixture]
    public class ClientTest
    {
        [TestCase]
        public void TestCloseIdleConnectionsNotPooled()
        {
            // No-op because done automatically by system.
        }

        [TestCase]
        public void TestCloseIdleConnectionsPooled()
        {
            // No-op because done automatically by system.
        }

        [TestCase]
        public void TestCloseExpiredConnectionsNotPooled()
        {
            // No-op because done automatically by system.
        }

        [TestCase]
        public void TestCloseExpiredConnectionsPooled()
        {
            // No-op because done automatically by system.
        }
    }
}
