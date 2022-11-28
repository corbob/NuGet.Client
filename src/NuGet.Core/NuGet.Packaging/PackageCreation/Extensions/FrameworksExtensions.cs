// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Versioning;
//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
using Chocolatey.NuGet.Frameworks;
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////

namespace NuGet.Packaging
{
    public static class FrameworksExtensions
    {
        // NuGet.Frameworks doesn't have the equivalent of the old VersionUtility.GetFrameworkString
        // which is relevant for building packages. This isn't needed for net5.0+ frameworks.
        public static string GetFrameworkString(this NuGetFramework self)
        {
            bool isNet5Era = (self.Version.Major >= 5 && StringComparer.OrdinalIgnoreCase.Equals(FrameworkConstants.FrameworkIdentifiers.NetCoreApp, self.Framework));
            if (isNet5Era)
            {
                return self.GetShortFolderName();
            }

            var frameworkName = new FrameworkName(self.DotNetFrameworkName);
            string name = frameworkName.Identifier + frameworkName.Version;
            if (string.IsNullOrEmpty(frameworkName.Profile))
            {
                return name;
            }
            return name + "-" + frameworkName.Profile;
        }
    }
}
