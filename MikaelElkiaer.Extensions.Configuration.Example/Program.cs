using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace MikaelElkiaer.Extensions.Configuration.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                //.AddBitwardenConfiguration() // This will enable substition of existing keys
                .AddBitwardenConfiguration(b => // Advanced configuration with complex secret types, as well as non-default options
                {
                    b.AddSecret(new Bitwarden.Model.SingleValueSecret("OVERRIDE_SINGLE_VALUE"));
                    b.AddSecret(new Bitwarden.Model.EnvFileSecret("multi_value_secret.env"));
                    b.AddSecret(new Bitwarden.Model.FieldsSecret("OVERRIDE_FIELDS", includeNotes: true, nameToFieldSeperator: "__"));
                    //b.DisableSubstituteExisting();
                    //b.SetSubstitutePrefix("bw:");
                })
                .Build();

            Console.WriteLine("Configuration key-value-pairs:");
            foreach (var c in configuration.GetChildren().Where(c => c.Key.StartsWith("OVERRIDE_")))
            {
               Console.WriteLine($"{c.Key}={c.Value}");
            }
        }
    }
}
