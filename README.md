# AnyPackage.DotNetTool

AnyPackage.DotNetTool is an AnyPackage provider that facilitates installing .NET Tools.

## Install AnyPackage.DotNetTool

```PowerShell
Install-PSResource AnyPackage.DotNetTool
```

## Import AnyPackage.DotNetTool

```PowerShell
Import-Module AnyPackage.DotNetTool
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

Uninstall-Package
```
