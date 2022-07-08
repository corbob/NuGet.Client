// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;

namespace NuGet.PackageManagement
{
    public partial class NuGetPackageManager
    {
        public event EventHandler<ChocolateyPackageOperationEventArgs> PackageInstalled;
        public event EventHandler<ChocolateyPackageOperationEventArgs> PackageUninstalled;
        public event EventHandler<ChocolateyPackageOperationEventArgs> PackageInstalling;
        public event EventHandler<ChocolateyPackageOperationEventArgs> PackageUninstalling;

        protected virtual void OnInstalled(ChocolateyPackageOperationEventArgs e)
        {
            if (PackageInstalled != null)
            {
                PackageInstalled(this, e);
            }
        }

        protected virtual void OnUninstalled(ChocolateyPackageOperationEventArgs e)
        {
            if (PackageUninstalled != null)
            {
                PackageUninstalled(this, e);
            }
        }

        protected virtual void OnInstalling(ChocolateyPackageOperationEventArgs e)
        {
            if (PackageInstalling != null)
            {
                PackageInstalling(this, e);
            }
        }

        protected virtual void OnUninstalling(ChocolateyPackageOperationEventArgs e)
        {
            if (PackageUninstalling != null)
            {
                PackageUninstalling(this, e);
            }
        }


    }
}
