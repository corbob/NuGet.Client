// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol.LocalRepositories
{
    public class LocalPackageListResourceProvider : ResourceProvider
    {

        public LocalPackageListResourceProvider() : base(
            typeof(ListResource),
            nameof(LocalPackageListResourceProvider),
            NuGetResourceProviderPositions.Last)
        {
        }

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source,
            CancellationToken token)
        {
            return await TryCreate(source, cacheContext: null, token);
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(SourceRepository source,
            SourceCacheContext cacheContext,
            CancellationToken token)
        {
            ListResource resource = null;
            var findLocalPackagesResource = await source.GetResourceAsync<FindLocalPackagesResource>(cacheContext, token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

            if (findLocalPackagesResource != null)
            {
                resource = new LocalPackageListResource(new LocalPackageSearchResource(findLocalPackagesResource), source.PackageSource.Source);
            }
            return new Tuple<bool, INuGetResource>(resource != null, resource);
        }
    }
}
