// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.PackageManagement
{
    public class UninstallationContext
    {
        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        // We need to have the constructor with optional parameters be
        // less available parameters than the other, to prevent a breaking change.
#pragma warning disable RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads.
        public UninstallationContext(bool removeDependencies = false,
#pragma warning restore RS0027 // Public API with optional parameter(s) should have the most parameters amongst its public overloads.
            bool forceRemove = false)
            : this(removeDependencies, forceRemove, warnDependencyResolvingFailure: false)
        {
        }

        public UninstallationContext(bool removeDependencies, bool forceRemove, bool warnDependencyResolvingFailure)
        {
            RemoveDependencies = removeDependencies;
            ForceRemove = forceRemove;
            WarnDependencyResolvingFailure = warnDependencyResolvingFailure;
        }

        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        /// <summary>
        /// Determines if dependencies should be uninstalled during package uninstall
        /// </summary>
        public bool RemoveDependencies { get; private set; }

        /// <summary>
        /// Determines if the package should be uninstalled forcefully even if it may break the build
        /// </summary>
        public bool ForceRemove { get; private set; }

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        public bool WarnDependencyResolvingFailure { get; private set; }

        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
    }
}
