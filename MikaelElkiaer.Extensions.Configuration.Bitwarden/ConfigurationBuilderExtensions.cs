using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using MikaelElkiaer.Extensions.Configuration.Bitwarden;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddBitwardenConfiguration(this IConfigurationBuilder builder, string secretName)
        {
            Secret secret;
            var lastDot = secretName.LastIndexOf('.');
            if (lastDot > -1 && secretName.Substring(lastDot) == ".env")
                secret = new EnvFileSecret(secretName);
            else
                secret = new SingleValueSecret(secretName);

            return AddBitwardenConfiguration(builder, secret);
        }

        public static IConfigurationBuilder AddBitwardenConfiguration(this IConfigurationBuilder builder, Secret secret)
        {
            return AddBitwardenConfiguration(builder, Enumerable.Repeat(secret, 1));
        }

        public static IConfigurationBuilder AddBitwardenConfiguration(this IConfigurationBuilder builder, IEnumerable<Secret> secrets)
        {
            return AddBitwardenConfiguration(builder, b => b.AddSecrets(secrets.ToArray()));
        }

        public static IConfigurationBuilder AddBitwardenConfiguration(this IConfigurationBuilder builder, Action<BitwardenConfigurationProviderOptionsBuilder> optionsBuilderDelegate)
        {
           var optionsBuilder = new BitwardenConfigurationProviderOptionsBuilder();
           optionsBuilderDelegate.Invoke(optionsBuilder);

           return AddBitwardenConfiguration(builder, optionsBuilder);
        }

        public static IConfigurationBuilder AddBitwardenConfiguration(this IConfigurationBuilder builder, BitwardenConfigurationProviderOptionsBuilder optionsBuilder)
        {
            return builder.Add(new BitwardenConfigurationSource(optionsBuilder.Build()));
        }
    }
}