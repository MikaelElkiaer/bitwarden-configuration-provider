using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using CliWrap;
using CliWrap.Buffered;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace MikaelElkiaer.Extensions.Configuration.Bitwarden
{
    public class BitwardenConfigurationProvider : ConfigurationProvider
    {
        private readonly BitwardenConfigurationProviderOptions options;

        public BitwardenConfigurationProvider(BitwardenConfigurationProviderOptions options)
        {
            this.options = options;
        }

        public override void Load()
        {
            string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string sessionKeyPath = Path.Combine(homePath, ".bw-session-key.tmp");
            string sessionKey = File.Exists(sessionKeyPath) ? File.ReadAllText(sessionKeyPath) : null;

            string statusResult;
            try
            {
                statusResult = CallCli(homePath, sessionKey, "status");
            }
            catch (Win32Exception)
            {
                throw new Exception("Missing dependency: `bw`");
            }

            var statusJson = JToken.Parse(statusResult);
            var status = statusJson["status"].Value<string>();

            if (status == "unauthenticated")
                throw new Exception("Unauthenticated - must login via `bw login` first");

            if (status == "locked")
            {
                var password = ReadHidden();
                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new Exception("Cannot unlock vault without password");
                }

                var unlockResult = (password | Cli.Wrap("bw").WithArguments("unlock --raw")).WithValidation(CommandResultValidation.None).ExecuteBufferedAsync().GetAwaiter().GetResult();
                if (unlockResult.ExitCode == 1)
                    throw new Exception("Something went wrong while unlocking Bitwarden - most likely incorrect password");
                ClearPasswordInput();
                sessionKey = unlockResult.StandardOutput.Trim();
                File.WriteAllText(sessionKeyPath, sessionKey);
            }
            statusResult = CallCli(homePath, sessionKey, "status");

            statusJson = JToken.Parse(statusResult);
            status = statusJson["status"].Value<string>();

            if (status != "unlocked")
                throw new Exception("Something went wrong");

            var keyValuePairs = new Dictionary<string, string>();
            foreach (var s in options.Secrets)
            {
                var secretResult = CallCli(homePath, sessionKey, "list", "items", "--search", s.Name);
                var secretJson = JArray.Parse(secretResult);
                if (secretJson.Count > 1)
                    throw new Exception($"Found multiple secrets with name {s.Name}");

                var item = secretJson[0]["notes"].Value<string>();

                switch (s)
                {
                    case EnvFileSecret envFileSecret:
                        var parsedItem = DotEnvFile.ParseLines(item.Split('\n'), true);
                        foreach (var i in parsedItem)
                            keyValuePairs[i.Key] = i.Value;
                        break;
                    case SingleValueSecret singleValueSecret:
                    default:
                        keyValuePairs[s.Name] = item;
                        break;
                }
            }

            Data = keyValuePairs;
        }

        private static string CallCli(string homePath, string sessionKey, string command, params string[] additionalArgs)
        {
            return Cli.Wrap("bw")
                    .WithArguments(b =>
                    {
                        if (!string.IsNullOrWhiteSpace(sessionKey))
                            b.Add(new[] { "--session", sessionKey });
                        b.Add(command);
                        foreach (var arg in additionalArgs)
                            b.Add(additionalArgs);
                    })
                    .WithWorkingDirectory(homePath)
                    .ExecuteBufferedAsync()
                    .GetAwaiter().GetResult()
                    .StandardOutput;
        }

        private string ReadHidden()
        {
            Console.Write("Enter Bitwarden password [input is hidden]: ");
            string password = "";
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Escape)
                    break;
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    password = password.Substring(0, password.Length - 2);
                password += key.KeyChar;
            }

            return password;
        }

        private void ClearPasswordInput()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write("Enter Bitwarden password: [hidden]         ");
            Console.WriteLine();
        }
    }
}
