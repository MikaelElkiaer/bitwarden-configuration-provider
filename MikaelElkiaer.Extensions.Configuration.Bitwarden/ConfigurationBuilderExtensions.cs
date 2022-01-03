using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using MikaelElkiaer.Extensions.Configuration.Bitwarden;
using MikaelElkiaer.Extensions.Configuration.Bitwarden.Model;
using MikaelElkiaer.Extensions.Configuration.Bitwarden.Options;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddBitwardenConfiguration(this IConfigurationBuilder builder, Action<BitwardenConfigurationProviderOptionsBuilder>? optionsBuilderDelegate = null)
        {
            var optionsBuilder = new BitwardenConfigurationProviderOptionsBuilder();
            optionsBuilderDelegate?.Invoke(optionsBuilder);

#if !DEBUG
            if (!(optionsBuilder.IsEnabledOutsideDebug() ?? false))
                return builder;
#endif

            var options = optionsBuilder.Build();

            IEnumerable<KeyValuePair<string, string>> existingKeyValues = Enumerable.Empty<KeyValuePair<string, string>>();
            if (!options.DisabledSubstituteExisting)
            {
                var tempConfig = builder.Build();
                existingKeyValues = tempConfig.AsEnumerable().Where(c => c.Value.StartsWith(options.SubstitutePrefix));
            }

            return builder.Add(new BitwardenConfigurationSource(options, existingKeyValues));
        }
    }
}
