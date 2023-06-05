// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class PackageSearchResourceV3Provider : ResourceProvider
    {
        public PackageSearchResourceV3Provider()
            : base(typeof(PackageSearchResource), nameof(PackageSearchResourceV3Provider), nameof(PackageSearchResourceV2FeedProvider))
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
            PackageSearchResourceV3 curResource = null;
            var serviceIndex = await source.GetResourceAsync<ServiceIndexResourceV3>(cacheContext, token);

            if (serviceIndex != null)
            {
                var endpoints = serviceIndex.GetServiceEntryUris(ServiceTypes.SearchQueryService);
                var httpSourceResource = await source.GetResourceAsync<HttpSourceResource>(cacheContext, token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

                // construct a new resource
                curResource = new PackageSearchResourceV3(httpSourceResource.HttpSource, endpoints);
            }

            return new Tuple<bool, INuGetResource>(curResource != null, curResource);
        }
    }
}
