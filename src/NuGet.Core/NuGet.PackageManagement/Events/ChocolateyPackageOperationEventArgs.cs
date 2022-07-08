// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace NuGet
{
    public class ChocolateyPackageOperationEventArgs : CancelEventArgs
    {
        public ChocolateyPackageOperationEventArgs(string installPath, IPackageMetadata package, string fileSystemRoot)
        {
            Package = package;
            InstallPath = installPath;
            FileSystemRoot = fileSystemRoot;
        }

        public string InstallPath { get; private set; }
        public IPackageMetadata Package { get; private set; }
        public string FileSystemRoot { get; private set; }
    }
}
