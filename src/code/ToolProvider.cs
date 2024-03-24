// Copyright (c) Thomas Nieto - All Rights Reserved
// You may use, distribute and modify this code under the
// terms of the MIT license.

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AnyPackage.Provider.DotNet
{
    [PackageProvider(".NET Tool")]
    public class ToolProvider : PackageProvider, IFindPackage, IGetPackage, IInstallPackage, IUpdatePackage, IUninstallPackage
    {
        protected override object? GetDynamicParameters(string commandName)
        {
            switch (commandName)
            {
                case "Install-Package":
                    return new InstallPackageDynamicParameters();
                
                case "Update-Package":
                    return new UpdatePackageDynamicParameters();

                default:
                    return null;
            }
        }
        
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
                        WritePackageVersions(request, dictionary, versions);
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
                WritePackageVersions(request, dictionary, versions);
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
                var match = Regex.Match(line, @"^(?<Name>\S+)\s+(?<Version>\d\S+)\s+(?<Commands>.+)$");

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

        public void InstallPackage(PackageRequest request)
        {
            var args = $"tool install {request.Name} --global";

            if (request.Version is not null)
            {
                args += $" --version {request.Version}";
            }

            if (request.Prerelease)
            {
                args += " --prerelease";
            }

            if (request.DynamicParameters is not null)
            {
                var dynamicParameters = request.DynamicParameters as InstallPackageDynamicParameters;

                if (dynamicParameters is not null && dynamicParameters.Architecture != "")
                {
                    args += $" --arch {dynamicParameters.Architecture}";
                }

                if (dynamicParameters is not null && dynamicParameters.Framework != "")
                {
                    args += $" --framework {dynamicParameters.Framework}";
                }
            }

            InvokeDotNet(request, args);
        }

        public void UninstallPackage(PackageRequest request)
        {
            var args = $"tool uninstall {request.Name} --global";
            InvokeDotNet(request, args);
        }

        public void UpdatePackage(PackageRequest request)
        {
            //TODO: Support wildcard
            var args = $"tool update {request.Name} --global";

            if (request.Version is not null)
            {
                args += $" --version {request.Version}";
            }

            if (request.Prerelease)
            {
                args += " --prerelease";
            }

            if (request.DynamicParameters is not null)
            {
                var dynamicParameters = request.DynamicParameters as UpdatePackageDynamicParameters;

                if (dynamicParameters is not null && dynamicParameters.Framework != "")
                {
                    args += $" --framework {dynamicParameters.Framework}";
                }
            }

            InvokeDotNet(request, args);
        }

        private void InvokeDotNet(PackageRequest request, string args)
        {
            request.WriteVerbose($"Calling 'dotnet {args}'");
            
            using var process = new Process();
            process.StartInfo.Arguments = args;
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            process.WaitForExit();

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            if (output.Length > 0)
            {
                request.WriteVerbose(output);
            }

            if (Regex.IsMatch(error, "could not be found"))
            {
                return;
            }
            else if (error.Length > 0)
            {
                throw new PackageProviderException(error);
            }

            var match = Regex.Match(output, @"Tool '(?<Name>\S+)'.+version '(?<Version>\S+)'");
            var package = new PackageInfo(match.Groups["Name"].Value,
                                          match.Groups["Version"].Value,
                                          ProviderInfo);

            request.WritePackage(package);
        }

        private void WritePackageVersions(PackageRequest request, Dictionary<string, object> dictionary, Dictionary<PackageVersion, long> versions)
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
