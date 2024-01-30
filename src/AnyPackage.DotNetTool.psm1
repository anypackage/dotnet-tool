# Copyright (c) Thomas Nieto - All Rights Reserved
# You may use, distribute and modify this code under the
# terms of the MIT license.

using module AnyPackage
using namespace AnyPackage.Provider
using namespace NuGet.Versioning
using namespace System.Management.Automation

[PackageProvider('DotNetTool')]
class DotNetToolProvider : PackageProvider {
    
}

[guid] $id = '52975d3f-362f-47a6-8730-322b1979b2ac'
[PackageProviderManager]::RegisterProvider($id, [ScoopProvider], $MyInvocation.MyCommand.ScriptBlock.Module)

$MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
    [PackageProviderManager]::UnregisterProvider($id)
}
