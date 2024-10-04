# AnyPackage.DotNet.Tool

[![gallery-image]][gallery-site]
[![build-image]][build-site]
[![cf-image]][cf-site]

[gallery-image]: https://img.shields.io/powershellgallery/dt/AnyPackage.DotNet.Tool?logo=powershell
[build-image]: https://img.shields.io/github/actions/workflow/status/anypackage/dotnet-tool/ci.yml
[cf-image]: https://img.shields.io/codefactor/grade/github/anypackage/dotnet-tool
[gallery-site]: https://www.powershellgallery.com/packages/AnyPackage.DotNet.Tool
[build-site]: https://github.com/anypackage/dotnet-tool/actions/workflows/ci.yml
[cf-site]: https://www.codefactor.io/repository/github/anypackage/dotnet-tool

AnyPackage.DotNet.Tool is an AnyPackage provider that facilitates installing .NET Tools.

## Install AnyPackage.DotNet.Tool

```PowerShell
Install-PSResource AnyPackage.DotNet.Tool
```

## Import AnyPackage.DotNet.Tool

```PowerShell
Import-Module AnyPackage.DotNet.Tool
```

## Sample usages

### Search for a package

```PowerShell
Find-Package -Name XMLDoc2Markdown

Find-Package -Name XMLDoc2Markdown*
```

### Install a package

```PowerShell
Find-Package XMLDoc2Markdown | Install-Package

Install-Package -Name XMLDoc2Markdown
```

### Get list of installed packages

```PowerShell
Get-Package -Name XMLDoc2Markdown
```

### Uninstall a package

```PowerShell
Get-Package -Name XMLDoc2Markdown | Uninstall-Package

Uninstall-Package -Name XMLDoc2Markdown
```

### Update a package

```PowerShell
Get-Package -Name XMLDoc2Markdown | Update-Package
```
