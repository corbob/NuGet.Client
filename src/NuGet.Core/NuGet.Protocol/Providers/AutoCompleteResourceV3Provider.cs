// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class AutoCompleteResourceV3Provider : ResourceProvider
    {
        public AutoCompleteResourceV3Provider()
            : base(typeof(AutoCompleteResource), nameof(AutoCompleteResourceV3Provider), nameof(AutoCompleteResourceV2FeedProvider))
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
            AutoCompleteResourceV3 curResource = null;

            var serviceIndex = await source.GetResourceAsync<ServiceIndexResourceV3>(cacheContext, token);

            if (serviceIndex != null)
            {
                var regResource = await source.GetResourceAsync<RegistrationResourceV3>(cacheContext, token);
                var httpSourceResource = await source.GetResourceAsync<HttpSourceResource>(cacheContext, token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

                // construct a new resource
                curResource = new AutoCompleteResourceV3(httpSourceResource.HttpSource, serviceIndex, regResource);
            }

            return new Tuple<bool, INuGetResource>(curResource != null, curResource);
        }
    }
}
