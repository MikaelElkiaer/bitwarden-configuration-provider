using System;
using System.Collections.Generic;
using static MikaelElkiaer.Extensions.Configuration.Bitwarden.BitwardenConfigurationProviderOptions;

namespace MikaelElkiaer.Extensions.Configuration.Bitwarden
{
    public class BitwardenConfigurationProviderOptionsBuilder
    {
        private readonly List<Secret> secrets = new List<Secret>();

        public void AddSecrets(params Secret[] secrets)
        {
            this.secrets.AddRange(secrets);
        }

        public BitwardenConfigurationProviderOptions Build()
        {
            return new BitwardenConfigurationProviderOptions(secrets);
        }
    }
}
