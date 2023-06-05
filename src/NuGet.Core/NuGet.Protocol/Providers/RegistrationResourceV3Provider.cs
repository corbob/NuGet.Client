// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class RegistrationResourceV3Provider : ResourceProvider
    {
        public RegistrationResourceV3Provider()
            : base(typeof(RegistrationResourceV3),
                  nameof(RegistrationResourceV3),
                  NuGetResourceProviderPositions.Last)
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
            RegistrationResourceV3 regResource = null;
            var serviceIndex = await source.GetResourceAsync<ServiceIndexResourceV3>(cacheContext, token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

            if (serviceIndex != null)
            {
                var baseUrl = serviceIndex.GetServiceEntryUri(ServiceTypes.RegistrationsBaseUrl);

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
                var httpSourceResource = await source.GetResourceAsync<HttpSourceResource>(cacheContext, token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

                // construct a new resource
                regResource = new RegistrationResourceV3(httpSourceResource.HttpSource, baseUrl);
            }

            return new Tuple<bool, INuGetResource>(regResource != null, regResource);
        }
    }
}
