// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    /// <summary>
    /// Retrieves all dependency info for the package resolver.
    /// </summary>
    public class DependencyInfoResourceV3Provider : ResourceProvider
    {
        public DependencyInfoResourceV3Provider()
            : base(typeof(DependencyInfoResource), nameof(DependencyInfoResourceV3Provider), "DependencyInfoResourceV2FeedProvider")
        {
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, CancellationToken token)
        {
        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
            return await TryCreate(source, cacheContext: null, token);
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source, SourceCacheContext cacheContext, CancellationToken token)
        {
            DependencyInfoResource curResource = null;

            if (await source.GetResourceAsync<ServiceIndexResourceV3>(cacheContext, token) != null)
            {
                var httpSourceResource = await source.GetResourceAsync<HttpSourceResource>(cacheContext, token);
                var regResource = await source.GetResourceAsync<RegistrationResourceV3>(cacheContext, token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

                // construct a new resource
                curResource = new DependencyInfoResourceV3(httpSourceResource.HttpSource, regResource, source);
            }

            return new Tuple<bool, INuGetResource>(curResource != null, curResource);
        }
    }
}
