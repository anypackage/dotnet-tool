// Copyright (c) Thomas Nieto - All Rights Reserved
// You may use, distribute and modify this code under the
// terms of the MIT license.

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AnyPackage.Provider.DotNet
{
    [PackageProvider(".NET Tool")]
    public class ToolProvider : PackageProvider, IFindPackage, IGetPackage
    {
        public void FindPackage(PackageRequest request)
        {
            if (request.Name == "*")
            {
                request.WriteVerbose(".NET Tool package provider requires a name.");
                return;
            }

            var args = $"tool search {request.Name} --detail";

            if (request.Prerelease)
            {
                args += " --prerelease";
            }

            request.WriteVerbose($"Calling 'dotnet {args}'");

            using var process = new Process();
            process.StartInfo.Arguments = args;
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            using var reader = process.StandardOutput;

            string? line;
            var first = true;
            var dictionary = new Dictionary<string, object>();
            var versions = new Dictionary<PackageVersion, long>();

            while ((line = reader.ReadLine()) is not null)
            {
                var match = Regex.Match(line, "^-+$");

                if (match.Success)
                {
                    if (!first)
                    {
                        dictionary["Versions"] = versions;
                        WritePackage(request, dictionary, versions);
                    }

                    first = false;
                    dictionary = [];
                    dictionary["Name"] = reader.ReadLine();
                    versions = [];
                    continue;
                }

                match = Regex.Match(line, @"^(?<Key>\S+): (?<Value>.*)$");

                if (match.Success && match.Groups["Key"].Value != "Versions")
                {
                    dictionary.Add(match.Groups["Key"].Value, match.Groups["Value"].Value);
                }

                match = Regex.Match(line, @"^\s+(?<Version>.+) Downloads: (?<Count>.+)$");

                if (match.Success)
                {
                    versions[match.Groups["Version"].Value] = long.Parse(match.Groups["Count"].Value);
                }
            }

            if (!first)
            {
                dictionary["Versions"] = versions;
                WritePackage(request, dictionary, versions);
            }
        }

        public void GetPackage(PackageRequest request)
        {
            using var process = new Process();
            process.StartInfo.Arguments = "tool list --global";
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            using var reader = process.StandardOutput;

            string? line;

            while ((line = reader.ReadLine()) is not null)
            {
                var match = Regex.Match(line, @"^(?<Name>\S+)\s+(?<Version>\d\\S+)\s+(?<Commands>.+)$");

                if (match.Success && request.IsMatch(match.Groups["Name"].Value, match.Groups["Version"].Value))
                {
                    var package = new PackageInfo(match.Groups["Name"].Value,
                                                  match.Groups["Version"].Value,
                                                  null,
                                                  "",
                                                  null,
                                                  new Dictionary<string, object>() { ["Commands"] = match.Groups["Commands"].Value.TrimEnd() },
                                                  ProviderInfo);

                    request.WritePackage(package);
                }
            }
        }

        private void WritePackage(PackageRequest request, Dictionary<string, object> dictionary, Dictionary<PackageVersion, long> versions)
        {
            foreach (var version in versions.Keys)
            {
                var package = new PackageInfo((string)dictionary["Name"],
                                              version,
                                              null,
                                              (string)dictionary["Description"],
                                              null,
                                              dictionary,
                                              ProviderInfo);


                if (request.IsMatch((string)dictionary["Name"], version))
                {
                    request.WritePackage(package);
                }
            }
        }
    }
}
