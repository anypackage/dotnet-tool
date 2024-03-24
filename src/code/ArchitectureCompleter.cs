// Copyright (c) Thomas Nieto - All Rights Reserved
// You may use, distribute and modify this code under the
// terms of the MIT license.

using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace AnyPackage.Provider.DotNet
{
    public sealed class ArchitectureCompleter : IArgumentCompleter
    {
        private static string[] architectures = new string[]
        {
            "android-arm64",
            "ios-arm64",
            "linux-arm",
            "linux-arm64",
            "linux-bionic-arm64",
            "linux-musl-arm64",
            "linux-musl-x64",
            "linux-x64",
            "osx-arm64",
            "osx-x64",
            "win-arm64",
            "win-x64",
            "win-x86"
        };

        public IEnumerable<CompletionResult> CompleteArgument(string commandName,
                                                              string parameterName,
                                                              string wordToComplete,
                                                              CommandAst commandAst,
                                                              IDictionary fakeBoundParameters)
        {
            var wildcard = new WildcardPattern(wordToComplete + "*", WildcardOptions.IgnoreCase);

            foreach (var arch in architectures)
            {
                if (wildcard.IsMatch(arch))
                {
                    yield return new CompletionResult(arch, arch, CompletionResultType.ParameterValue, arch);
                }
            }
        }
    }
}
