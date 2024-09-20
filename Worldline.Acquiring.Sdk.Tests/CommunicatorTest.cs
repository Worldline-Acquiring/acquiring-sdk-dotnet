using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using Worldline.Acquiring.Sdk.Authentication;
using Worldline.Acquiring.Sdk.Communication;
using Worldline.Acquiring.Sdk.Json;

namespace Worldline.Acquiring.Sdk
{
    [TestFixture]
    public class CommunicatorTest
    {
        private readonly Uri _baseUri = new Uri("https://api.preprod.acquiring.worldline-solutions.com");
        private readonly Mock<IConnection> _connectionMock = new Mock<IConnection>();
        private readonly Mock<IAuthenticator> _authenticatorMock = new Mock<IAuthenticator>();
        private readonly MetadataProvider _metadataProvider = new MetadataProvider("Worldline");
        private readonly Mock<IMarshaller> _marshallerMock = new Mock<IMarshaller>();

        [TestCase]
        public void TestToUriWithoutRequestParams()
        {
            var communicator = new Communicator(_baseUri, _connectionMock.Object, _authenticatorMock.Object, _metadataProvider, _marshallerMock.Object);
            var uri = communicator.ToAbsoluteUri("services/v1/100812/520000214/dcc-rates", new List<RequestParam>());
            var uri2 = communicator.ToAbsoluteUri("/services/v1/100812/520000214/dcc-rates", new List<RequestParam>());

            Assert.That(uri, Is.EqualTo(new Uri("https://api.preprod.acquiring.worldline-solutions.com/services/v1/100812/520000214/dcc-rates")));
            Assert.That(uri2, Is.EqualTo(new Uri("https://api.preprod.acquiring.worldline-solutions.com/services/v1/100812/520000214/dcc-rates")));
        }

        [TestCase]
        public void TestToUriWithRequestParams()
        {
            var list = new List<RequestParam>
            {
                new RequestParam("amount", "123"),
                new RequestParam("source", "USD"),
                new RequestParam("target", "EUR"),
                new RequestParam("dummy", "é&%=")
            };
            var communicator = new Communicator(_baseUri, _connectionMock.Object, _authenticatorMock.Object, _metadataProvider, _marshallerMock.Object);
            var uri = communicator.ToAbsoluteUri("services/v1/100812/520000214/dcc-rates", list);
            var uri2 = communicator.ToAbsoluteUri("/services/v1/100812/520000214/dcc-rates", list);

            Assert.AreEqual(new Uri("https://api.preprod.acquiring.worldline-solutions.com/services/v1/100812/520000214/dcc-rates?amount=123&source=USD&target=EUR&dummy=%C3%A9%26%25%3D"), uri);
            Assert.AreEqual(new Uri("https://api.preprod.acquiring.worldline-solutions.com/services/v1/100812/520000214/dcc-rates?amount=123&source=USD&target=EUR&dummy=%C3%A9%26%25%3D"), uri2);
        }
    }
}
