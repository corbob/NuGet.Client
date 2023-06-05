// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// A resource for detecting the capabilities of a V2 feed.
    /// </summary>
    public abstract class LegacyFeedCapabilityResource : INuGetResource, ILegacyFeedCapabilityResource
    {
        public abstract Task<bool> SupportsSearchAsync(ILogger log, CancellationToken token);

        public abstract Task<bool> SupportsIsAbsoluteLatestVersionAsync(ILogger log, CancellationToken token);

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        public abstract Task<bool> SupportsSearchAsync(ILogger log, SourceCacheContext cacheContext, CancellationToken token);

        public abstract Task<bool> SupportsIsAbsoluteLatestVersionAsync(ILogger log, SourceCacheContext cacheContext, CancellationToken token);

        public abstract Task<bool> SupportsFindPackagesByIdAsync(ILogger log, CancellationToken token);

        public abstract Task<bool> SupportsFindPackagesByIdAsync(ILogger log, SourceCacheContext cacheContext, CancellationToken token);

        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
    }
}
