using System;
using System.Collections.Generic;
using System.Linq;
using MikaelElkiaer.Extensions.Configuration.Bitwarden.Model;

namespace MikaelElkiaer.Extensions.Configuration.Bitwarden.Options
{
    public class BitwardenConfigurationProviderOptions
    {
        public BitwardenConfigurationProviderOptions(IEnumerable<Secret>? secrets, bool? disabledSubstituteExisting, string? substitutePrefix)
        {
            if (secrets != null)
                Secrets = secrets;
            if (disabledSubstituteExisting.HasValue)
                DisabledSubstituteExisting = disabledSubstituteExisting.Value;
            if (substitutePrefix != null)
                SubstitutePrefix = substitutePrefix;
        }

        public IEnumerable<Secret> Secrets { get; } = Enumerable.Empty<Secret>();
        public bool DisabledSubstituteExisting { get; } = false;
        public string SubstitutePrefix { get; } = "bw:";
    }
}
