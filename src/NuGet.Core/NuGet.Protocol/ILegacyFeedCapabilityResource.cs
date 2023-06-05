// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;

namespace NuGet.Protocol.Core.Types
{
    public interface ILegacyFeedCapabilityResource
    {
        Task<bool> SupportsIsAbsoluteLatestVersionAsync(ILogger log, CancellationToken token);
        Task<bool> SupportsSearchAsync(ILogger log, CancellationToken token);

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        Task<bool> SupportsIsAbsoluteLatestVersionAsync(ILogger log, SourceCacheContext cacheContext, CancellationToken token);
        Task<bool> SupportsSearchAsync(ILogger log, SourceCacheContext cacheContext, CancellationToken token);
        Task<bool> SupportsFindPackagesByIdAsync(ILogger log, CancellationToken token);
        Task<bool> SupportsFindPackagesByIdAsync(ILogger log, SourceCacheContext cacheContext, CancellationToken token);

        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
    }
}
