// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Protocol.Core.Types
{
    public enum SearchOrderBy
    {
        /// <summary>
        /// Order the resulting packages by package ID.
        /// </summary>
        Id,

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        /// <summary>
        /// Order the resulting packages by number of downloads.
        /// </summary>
        DownloadCount,

        /// <summary>
        /// Order the resulting packages grouped by the version number
        /// </summary>
        Version,

        /// <summary>
        /// Order the resulting packages by number of downloads, and also group by the version number
        /// </summary>
        DownloadCountAndVersion

        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
    }
}
