using NUnit.Framework;

namespace Worldline.Acquiring.Sdk.Logging
{
    [TestFixture]
    public class BodyObfuscatorTest
    {
        private const string NoObfuscation = @"{
    ""amount"": {
        ""currencyCode"": ""EUR"",
        ""amount"": 1000
    },
    ""authorizationType"": ""PRE_AUTHORIZATION""
}";
        private const string BinObfuscated = @"{
    ""bin"": ""123456**""
}";
        private const string BinUnobfuscated = @"{
    ""bin"": ""12345678""
}";

        private const string CardObfuscated = @"{
    ""amount"": {
        ""currencyCode"": ""CAD"",
        ""amount"": 2345
    },
    ""cardPaymentData"": {
        ""cardData"": {
            ""cardSecurityCode"": ""***"",
            ""cardNumber"": ""************3456"",
            ""expiryDate"": ""**2024""
        }
    }
}";
        private const string CardObfuscatedCustom = @"{
    ""amount"": {
        ""currencyCode"": ""CAD"",
        ""amount"": 2345
    },
    ""cardPaymentData"": {
        ""cardData"": {
            ""cardSecurityCode"": ""***"",
            ""cardNumber"": ""123456******3456"",
            ""expiryDate"": ""**2024""
        }
    }
}";
        private const string CardUnObfuscated = @"{
    ""amount"": {
        ""currencyCode"": ""CAD"",
        ""amount"": 2345
    },
    ""cardPaymentData"": {
        ""cardData"": {
            ""cardSecurityCode"": ""123"",
            ""cardNumber"": ""1234567890123456"",
            ""expiryDate"": ""122024""
        }
    }
}";

        private const string NoObjectObfuscationUnobfuscated = @"{
    ""name"" : true,
    ""name"" : {
    }
}";
        private const string NoObjectObfuscationObfuscated = @"{
    ""name"" : ****,
    ""name"" : {
    }
}";

        [TestCase]
        public void TestObfuscateBodyWithNullBody()
        {
            var obfuscatedBody = BodyObfuscator.DefaultObfuscator.ObfuscateBody(null);

            Assert.Null(obfuscatedBody);
        }

        [TestCase]
        public void TestObfuscateBodyWithEmptyBody()
        {
            const string body = "";

            var obfuscatedBody = BodyObfuscator.DefaultObfuscator.ObfuscateBody(body);

            Assert.AreEqual(body, obfuscatedBody);
        }

        [TestCase]
        public void TestObfuscateBodyWithCard()
        {
            CheckObfuscatedBodyWithMatches(CardUnObfuscated, CardObfuscated);
        }

        [TestCase]
        public void TestObfuscateBodyWithCustomCardRule()
        {
            var bodyObfuscator = BodyObfuscator.Custom()
                    .ObfuscateCustom("cardNumber", KeepFirst6AndLast4)
                    .Build();

            CheckObfuscatedBodyWithMatches(bodyObfuscator, CardUnObfuscated, CardObfuscatedCustom);
        }

        [TestCase]
        public void TestObfuscateBodyWithBin()
        {
            CheckObfuscatedBodyWithMatches(BinUnobfuscated, BinObfuscated);
        }

        [TestCase]
        public void TestObfuscateBodyWithNoMatches()
        {
            CheckObfuscatedBodyWithNoMatches(NoObfuscation);
        }

        [TestCase]
        public void TestObfuscateBodyWithObject()
        {
            CheckObfuscatedBodyWithMatches(NoObjectObfuscationUnobfuscated, NoObjectObfuscationObfuscated);
        }

        private static void CheckObfuscatedBodyWithMatches(string body, string expected)
        {
            CheckObfuscatedBodyWithMatches(BodyObfuscator.DefaultObfuscator, body, expected);
        }

        private static void CheckObfuscatedBodyWithMatches(BodyObfuscator bodyObfuscator, string body, string expected)
        {
            var obfuscatedBody = bodyObfuscator.ObfuscateBody(body);

            Assert.AreEqual(expected, obfuscatedBody);
        }

        private static void CheckObfuscatedBodyWithNoMatches(string body)
        {
            var obfuscatedBody = BodyObfuscator.DefaultObfuscator.ObfuscateBody(body);

            Assert.AreEqual(body, obfuscatedBody);
        }

        private static string KeepFirst6AndLast4(string value)
        {
            var chars = value.ToCharArray();
            for (var i = 6; i < chars.Length - 4; i++)
            {
                chars[i] = '*';
            }
            return new string(chars);
        }
    }
}
