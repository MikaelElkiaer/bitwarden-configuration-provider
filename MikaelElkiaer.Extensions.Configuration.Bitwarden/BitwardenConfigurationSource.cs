using System;
using Microsoft.Extensions.Configuration;

namespace MikaelElkiaer.Extensions.Configuration.Bitwarden
{
    public class BitwardenConfigurationSource : IConfigurationSource
    {
        private readonly BitwardenConfigurationProviderOptions options;

        public BitwardenConfigurationSource(BitwardenConfigurationProviderOptions options)
        {
            this.options = options;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new BitwardenConfigurationProvider(options);
        }
    }
}
