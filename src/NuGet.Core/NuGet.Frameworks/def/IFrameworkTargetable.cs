// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
namespace Chocolatey.NuGet.Frameworks
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
{
    /// <summary>
    /// Use this to expose the list of target frameworks an object can be used for.
    /// </summary>
    public interface IFrameworkTargetable
    {
        /// <summary>
        /// All frameworks supported by the parent
        /// </summary>
        IEnumerable<NuGetFramework> SupportedFrameworks { get; }
    }
}
