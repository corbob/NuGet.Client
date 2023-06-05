// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class RepositorySignatureResourceProvider : ResourceProvider
    {
        public RepositorySignatureResourceProvider()
           : base(typeof(RepositorySignatureResource),
                 nameof(RepositorySignatureResource),
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
            RepositorySignatureResource resource = null;
            var serviceIndex = await source.GetResourceAsync<ServiceIndexResourceV3>(cacheContext, token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
            if (serviceIndex != null)
            {
                var serviceEntry = serviceIndex.GetServiceEntries(ServiceTypes.RepositorySignatures).FirstOrDefault();

                if (serviceEntry != null)
                {
        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
                    resource = await GetRepositorySignatureResourceAsync(source, serviceEntry, NullLogger.Instance, cacheContext, token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
                }
            }

            return new Tuple<bool, INuGetResource>(resource != null, resource);
        }

        private async Task<RepositorySignatureResource> GetRepositorySignatureResourceAsync(
            SourceRepository source,
            ServiceIndexEntry serviceEntry,
            ILogger log,
            CancellationToken token)
        {
        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
            return await GetRepositorySignatureResourceAsync(source, serviceEntry, log, cacheContext: null, token);
        }

        private async Task<RepositorySignatureResource> GetRepositorySignatureResourceAsync(
            SourceRepository source,
            ServiceIndexEntry serviceEntry,
            ILogger log,
            SourceCacheContext cacheContext,
            CancellationToken token)
        {
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
            var repositorySignaturesResourceUri = serviceEntry.Uri;

            if (repositorySignaturesResourceUri == null || !string.Equals(repositorySignaturesResourceUri.Scheme, "https", StringComparison.OrdinalIgnoreCase))
            {
                throw new FatalProtocolException(string.Format(CultureInfo.CurrentCulture, Strings.RepositorySignaturesResourceMustBeHttps, source.PackageSource.Source));
            }

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
            var httpSourceResource = await source.GetResourceAsync<HttpSourceResource>(cacheContext, token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
            var client = httpSourceResource.HttpSource;
            var cacheKey = GenerateCacheKey(serviceEntry);

            const int maxRetries = 3;
            for (var retry = 1; retry <= maxRetries; retry++)
            {
                using (var sourceCacheContext = new SourceCacheContext())
                {
        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
                    var httpSourceCacheContext = HttpSourceCacheContext.Create(sourceCacheContext, isFirstAttempt: retry == 1);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

                    try
                    {
                        return await client.GetAsync(
                            new HttpSourceCachedRequest(
                                serviceEntry.Uri.AbsoluteUri,
                                cacheKey,
        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
                                httpSourceCacheContext)
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
                            {
                                EnsureValidContents = stream => HttpStreamValidation.ValidateJObject(repositorySignaturesResourceUri.AbsoluteUri, stream),
                                MaxTries = 1,
                                IsRetry = retry > 1,
                                IsLastAttempt = retry == maxRetries
                            },
                            async httpSourceResult =>
                            {
                                var json = await httpSourceResult.Stream.AsJObjectAsync(token);

                                return new RepositorySignatureResource(json, source);
                            },
                            log,
                            token);
                    }
                    catch (Exception ex) when (retry < maxRetries)
                    {
                        var message = string.Format(CultureInfo.CurrentCulture, Strings.Log_RetryingRepositorySignature, repositorySignaturesResourceUri.AbsoluteUri)
                            + Environment.NewLine
                            + ExceptionUtilities.DisplayMessage(ex);
                        log.LogMinimal(message);
                    }
                    catch (Exception ex) when (retry == maxRetries)
                    {
                        var message = string.Format(CultureInfo.CurrentCulture, Strings.Log_FailedToReadRepositorySignature, repositorySignaturesResourceUri.AbsoluteUri);

                        throw new FatalProtocolException(message, ex);
                    }
                }
            }

            return null;
        }

        private static string GenerateCacheKey(ServiceIndexEntry serviceEntry)
        {
#if NETCOREAPP
            var index = serviceEntry.Type.IndexOf('/', StringComparison.Ordinal);
#else
            var index = serviceEntry.Type.IndexOf('/');
#endif
            var version = serviceEntry.Type.Substring(index + 1).Trim();

            return $"repository_signatures_{version}";
        }
    }
}
