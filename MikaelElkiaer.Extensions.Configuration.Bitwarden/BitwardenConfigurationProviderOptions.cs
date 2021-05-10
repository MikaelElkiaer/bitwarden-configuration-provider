using System;
using System.Collections.Generic;

namespace MikaelElkiaer.Extensions.Configuration.Bitwarden
{
    public partial class BitwardenConfigurationProviderOptions
    {
        public BitwardenConfigurationProviderOptions(IEnumerable<Secret> secrets)
        {
            Secrets = secrets;
        }

        public IEnumerable<Secret> Secrets { get; }
    }
}
