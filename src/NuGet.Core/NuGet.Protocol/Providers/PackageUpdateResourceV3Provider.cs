// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class PackageUpdateResourceV3Provider : ResourceProvider
    {
        public PackageUpdateResourceV3Provider()
            : base(
                  typeof(PackageUpdateResource),
                  nameof(PackageUpdateResourceV3Provider),
                  "PushCommandResourceV2Provider")
        { }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(
            SourceRepository source,
            CancellationToken token)
        {
        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
            return await TryCreate(source, cacheContext: null, token);
        }

        public override async Task<Tuple<bool, INuGetResource>> TryCreate(
            SourceRepository source,
            SourceCacheContext cacheContext,
            CancellationToken token)
        {
            PackageUpdateResource packageUpdateResource = null;

            var serviceIndex = await source.GetResourceAsync<ServiceIndexResourceV3>(cacheContext, token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

            if (serviceIndex != null)
            {
                var baseUrl = serviceIndex.GetServiceEntryUri(ServiceTypes.PackagePublish);

                //////////////////////////////////////////////////////////
                // Start - Chocolatey Specific Modification
                //////////////////////////////////////////////////////////
                IHttpSource httpSource = null;
                //////////////////////////////////////////////////////////
                // End - Chocolatey Specific Modification
                //////////////////////////////////////////////////////////
                var sourceUri = baseUrl?.AbsoluteUri;
                if (!string.IsNullOrEmpty(sourceUri))
                {
                    if (!(new Uri(sourceUri)).IsFile)
                    {
        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
                        var httpSourceResource = await source.GetResourceAsync<HttpSourceResource>(cacheContext, token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
                        httpSource = httpSourceResource.HttpSource;
                    }
                    packageUpdateResource = new PackageUpdateResource(sourceUri, httpSource);
                }
                else
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                        Strings.PackageServerEndpoint_NotSupported,
                        source));
                }
            }

            var result = new Tuple<bool, INuGetResource>(packageUpdateResource != null, packageUpdateResource);
            return result;
        }
    }
}
