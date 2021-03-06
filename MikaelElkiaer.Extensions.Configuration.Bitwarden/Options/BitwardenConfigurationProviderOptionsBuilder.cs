using System;
using System.Collections.Generic;
using MikaelElkiaer.Extensions.Configuration.Bitwarden.Model;

namespace MikaelElkiaer.Extensions.Configuration.Bitwarden.Options
{
    public class BitwardenConfigurationProviderOptionsBuilder
    {
        private List<Secret> secrets = new List<Secret>();
        private bool? disabledSubstiteExisting;
        private string? substitutePrefix;

        public BitwardenConfigurationProviderOptionsBuilder AddSecret(Secret secret)
        {
            this.secrets.Add(secret);

            return this;
        }

        public BitwardenConfigurationProviderOptionsBuilder AddSecrets(params Secret[] secrets)
        {
            this.secrets.AddRange(secrets);

            return this;
        }
        
        public BitwardenConfigurationProviderOptionsBuilder DisableSubstituteExisting()
        {
            disabledSubstiteExisting = false;

            return this;
        }

        public BitwardenConfigurationProviderOptionsBuilder SetSubstitutePrefix(string prefix)
        {
            substitutePrefix = prefix;

            return this;
        }

        public BitwardenConfigurationProviderOptions Build()
        {
            return new BitwardenConfigurationProviderOptions(secrets, disabledSubstiteExisting, substitutePrefix);
        }
    }
}
