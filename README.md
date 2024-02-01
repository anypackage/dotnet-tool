# AnyPackage.DotNet.Tool

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
