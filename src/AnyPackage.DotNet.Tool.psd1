@{
    RootModule = 'AnyPackage.DotNet.ToolProvider.dll'
    ModuleVersion = '0.1.0'
    CompatiblePSEditions = @('Desktop', 'Core')
    GUID = '2f27ec2e-002e-4145-8fd1-8959884ec06b'
    Author = 'Thomas Nieto'
    Copyright = '(c) 2024 Thomas Nieto. All rights reserved.'
    Description = '.NET Tool provider for AnyPackage.'
    PowerShellVersion = '5.1'
    RequiredModules = @(
        @{ ModuleName = 'AnyPackage'; ModuleVersion = '0.5.1' }
    )
    FunctionsToExport = @()
    CmdletsToExport = @()
    AliasesToExport = @()
    PrivateData = @{
        AnyPackage = @{
            Providers = '.NET Tool'
        }
        PSData = @{
            Tags = @('AnyPackage', 'Provider', 'DotNet', 'Tool', 'Windows', 'Linux', "MacOS")
            LicenseUri = 'https://github.com/anypackage/dotnet-tool/blob/main/LICENSE'
            ProjectUri = 'https://github.com/anypackage/dotnet-tool'
        }
    }
    HelpInfoURI = 'https://go.anypackage.dev/help'
}
