// Copyright (c) Thomas Nieto - All Rights Reserved
// You may use, distribute and modify this code under the
// terms of the MIT license.

using System;
using System.Management.Automation;
using static AnyPackage.Provider.PackageProviderManager;

namespace AnyPackage.Provider.DotNet
{
    public sealed class Init : IModuleAssemblyInitializer, IModuleAssemblyCleanup
    {
        private readonly Guid _id = new Guid("6bce96b5-446f-431a-ad7c-683dbc6d4aac");
        
        public void OnImport()
        {
            RegisterProvider(_id, typeof(ToolProvider), "AnyPackage.DotNet.Tool");
        }

        public void OnRemove(PSModuleInfo psModuleInfo)
        {
            UnregisterProvider(_id);
        }
    }
}
