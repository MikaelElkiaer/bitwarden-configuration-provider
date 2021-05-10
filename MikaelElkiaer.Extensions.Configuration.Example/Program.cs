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
                .AddBitwardenConfiguration("test.env")
                .AddEnvironmentVariables()
                .Build();

            Console.WriteLine("Configuration key-value-pairs:");
            foreach (var c in configuration.GetChildren())
            {
               Console.WriteLine($"{c.Key}={c.Value}");
            }
        }
    }
}
