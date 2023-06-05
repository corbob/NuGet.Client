// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class LocalPackageSearchResourceProvider : ResourceProvider
    {
        public LocalPackageSearchResourceProvider()
            : base(typeof(PackageSearchResource), nameof(LocalPackageSearchResourceProvider), NuGetResourceProviderPositions.Last)
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
            INuGetResource resource = null;

            var localResource = await source.GetResourceAsync<FindLocalPackagesResource>(cacheContext, token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

            if (localResource != null)
            {
                resource = new LocalPackageSearchResource(localResource);
            }

            return new Tuple<bool, INuGetResource>(resource != null, resource);
        }
    }
}
