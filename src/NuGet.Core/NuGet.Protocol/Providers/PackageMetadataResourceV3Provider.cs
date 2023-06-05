// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class PackageMetadataResourceV3Provider : ResourceProvider
    {
        public PackageMetadataResourceV3Provider()
            : base(typeof(PackageMetadataResource), nameof(PackageMetadataResourceV3Provider), nameof(PackageMetadataResourceV2FeedProvider))
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
            PackageMetadataResourceV3 curResource = null;

            if (await source.GetResourceAsync<ServiceIndexResourceV3>(cacheContext, token) != null)
            {
                var regResource = await source.GetResourceAsync<RegistrationResourceV3>();
                var reportAbuseResource = await source.GetResourceAsync<ReportAbuseResourceV3>();
                var packageDetailsUriResource = await source.GetResourceAsync<PackageDetailsUriResourceV3>();

                var httpSourceResource = await source.GetResourceAsync<HttpSourceResource>(cacheContext, token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

                // construct a new resource
                curResource = new PackageMetadataResourceV3(
                    httpSourceResource.HttpSource,
                    regResource,
                    reportAbuseResource,
                    packageDetailsUriResource);
            }

            return new Tuple<bool, INuGetResource>(curResource != null, curResource);
        }
    }
}
