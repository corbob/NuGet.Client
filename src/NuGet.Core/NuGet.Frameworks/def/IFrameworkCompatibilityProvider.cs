// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
namespace Chocolatey.NuGet.Frameworks
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
{
    public interface IFrameworkCompatibilityProvider
    {
        /// <summary>
        /// Ex: IsCompatible(net45, net40) -> true
        /// Ex: IsCompatible(net40, net45) -> false
        /// </summary>
        /// <param name="framework">Project target framework</param>
        /// <param name="other">Library framework that is going to be installed</param>
        /// <returns>True if framework supports other</returns>
        bool IsCompatible(NuGetFramework framework, NuGetFramework other);
    }
}
