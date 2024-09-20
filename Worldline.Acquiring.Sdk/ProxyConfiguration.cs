using System.Configuration;

namespace Worldline.Acquiring.Sdk
{
    internal class ProxyConfiguration : UriConfiguration
    {
        [ConfigurationProperty("username", IsRequired = false)]
        public string Username => ((string)this["username"]).NullIfEmpty();

        [ConfigurationProperty("password", IsRequired = false)]
        public string Password => ((string)this["password"]).NullIfEmpty();
    }
}
