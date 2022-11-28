// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
using Chocolatey.NuGet.Frameworks;
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
using NuGet.Packaging;

namespace NuGet.PackageManagement.UI
{
    public class PackageDependencySetMetadata
    {
        public PackageDependencySetMetadata(PackageDependencyGroup dependencyGroup)
        {
            if (dependencyGroup == null)
            {
                TargetFrameworkDisplay = Resources.Text_NoDependencies;
            }
            else
            {
                TargetFramework = dependencyGroup.TargetFramework;
                TargetFrameworkDisplay = TargetFramework.ToString();

                if (dependencyGroup.Packages.Any())
                {
                    Dependencies = dependencyGroup.Packages
                        .Select(d => new PackageDependencyMetadata(d))
                        .ToList()
                        .AsReadOnly();
                }
                else
                {
                    Dependencies = NoDependenciesPlaceholder;
                }
            }
        }

        public NuGetFramework TargetFramework { get; private set; }
        public string TargetFrameworkDisplay { get; private set; }
        public IReadOnlyCollection<PackageDependencyMetadata> Dependencies { get; private set; }
        private static readonly IReadOnlyCollection<PackageDependencyMetadata> NoDependenciesPlaceholder = new PackageDependencyMetadata[] { new PackageDependencyMetadata() { IsNoDependencyPlaceHolder = true } };
    }
}
