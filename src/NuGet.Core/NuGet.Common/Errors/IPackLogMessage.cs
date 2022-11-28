// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
using Chocolatey.NuGet.Frameworks;
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////

namespace NuGet.Common
{
    public interface IPackLogMessage : INuGetLogMessage
    {
        /// <summary>
        /// Project or Package Id.
        /// </summary>
        string LibraryId { get; set; }

        /// <summary>
        /// NuGet Framework
        /// </summary>
        public NuGetFramework Framework { get; set; }
    }
}
