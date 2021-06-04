using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MikaelElkiaer.Extensions.Configuration.Bitwarden.Options;

namespace MikaelElkiaer.Extensions.Configuration.Bitwarden
{
    public class BitwardenConfigurationSource : IConfigurationSource
    {
        private readonly BitwardenConfigurationProviderOptions options;
        private readonly IEnumerable<KeyValuePair<string, string>> existingKeyValues;

        public BitwardenConfigurationSource(BitwardenConfigurationProviderOptions options, IEnumerable<KeyValuePair<string, string>> existingKeyValues)
        {
            this.options = options;
            this.existingKeyValues = existingKeyValues;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new BitwardenConfigurationProvider(options, existingKeyValues);
        }
    }
}
