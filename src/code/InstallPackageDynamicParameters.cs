// Copyright (c) Thomas Nieto - All Rights Reserved
// You may use, distribute and modify this code under the
// terms of the MIT license.

using System.Management.Automation;

namespace AnyPackage.Provider.DotNet
{
    public sealed class InstallPackageDynamicParameters
    {
        [Parameter]
        [ArgumentCompleter(typeof(ArchitectureCompleter))]
        [ValidateNotNullOrEmpty]
        public string Architecture { get; set; } = string.Empty;

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string Framework { get; set; } = string.Empty;
    }
}
