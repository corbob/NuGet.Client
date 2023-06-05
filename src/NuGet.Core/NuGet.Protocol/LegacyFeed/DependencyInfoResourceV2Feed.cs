// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
using Chocolatey.NuGet.Frameworks;
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    public class DependencyInfoResourceV2Feed : DependencyInfoResource
    {
        private readonly V2FeedParser _feedParser;
        private readonly FrameworkReducer _frameworkReducer = new FrameworkReducer();
        private readonly SourceRepository _source;

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        private readonly ILegacyFeedCapabilityResource _feedCapabilities;

        public DependencyInfoResourceV2Feed(V2FeedParser feedParser, ILegacyFeedCapabilityResource feedCapabilities, SourceRepository source)
        {
            if (feedParser == null)
            {
                throw new ArgumentNullException(nameof(feedParser));
            }

            _feedParser = feedParser;
            _source = source;
            _feedCapabilities = feedCapabilities;
        }

        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        public override async Task<SourcePackageDependencyInfo> ResolvePackage(
            PackageIdentity package,
            NuGetFramework projectFramework,
            SourceCacheContext sourceCacheContext,
            ILogger log,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            try
            {
                var packageInfo = await _feedParser.GetPackage(package, sourceCacheContext, log, token);

                if (packageInfo == null)
                {
                    return null;
                }
                return CreateDependencyInfo(packageInfo, projectFramework);
            }
            catch (Exception ex)
            {
                // Wrap exceptions coming from the server with a user friendly message
                var error = String.Format(CultureInfo.CurrentCulture, Strings.Protocol_PackageMetadataError, package, _source);

                throw new FatalProtocolException(error, ex);
            }

        }

        public override async Task<IEnumerable<SourcePackageDependencyInfo>> ResolvePackages(
            string packageId,
            NuGetFramework projectFramework,
            SourceCacheContext sourceCacheContext,
            ILogger log,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            try
            {
                //////////////////////////////////////////////////////////
                // Start - Chocolatey Specific Modification
                //////////////////////////////////////////////////////////
                var packages = await FindPackageById(packageId, includeUnlisted: true, includePrerelease: true, sourceCacheContext, log, token);
                //////////////////////////////////////////////////////////
                // End - Chocolatey Specific Modification
                //////////////////////////////////////////////////////////

                var results = new List<SourcePackageDependencyInfo>();

                foreach (var package in packages)
                {
                    results.Add(CreateDependencyInfo(package, projectFramework));
                }
                return results;
            }
            catch (Exception ex)
            {
                // Wrap exceptions coming from the server with a user friendly message
                var error = String.Format(CultureInfo.CurrentCulture, Strings.Protocol_PackageMetadataError, packageId, _source);

                throw new FatalProtocolException(error, ex);
            }
        }

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
        public override async Task<IEnumerable<SourcePackageDependencyInfo>> ResolvePackages(
            string packageId,
            bool includePrerelease,
            NuGetFramework projectFramework,
            SourceCacheContext sourceCacheContext,
            ILogger log,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            try
            {
                var packages = await FindPackageById(packageId, true, includePrerelease, sourceCacheContext, log, token);

                var results = new List<SourcePackageDependencyInfo>();

                foreach (var package in packages)
                {
                    results.Add(CreateDependencyInfo(package, projectFramework));
                }
                return results;
            }
            catch (Exception ex)
            {
                // Wrap exceptions coming from the server with a user friendly message
                var error = String.Format(CultureInfo.CurrentCulture, Strings.Protocol_PackageMetadataError, packageId, _source);

                throw new FatalProtocolException(error, ex);
            }
        }

        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        /// <summary>
        /// Convert a V2 feed package into a V3 PackageDependencyInfo
        /// </summary>
        private SourcePackageDependencyInfo CreateDependencyInfo(
            V2FeedPackageInfo packageVersion,
            NuGetFramework projectFramework)
        {
            var deps = Enumerable.Empty<PackageDependency>();

            var identity = new PackageIdentity(packageVersion.Id, NuGetVersion.Parse(packageVersion.Version.ToString()));
            if (packageVersion.DependencySets != null
                && packageVersion.DependencySets.Any())
            {
                // Take only the dependency group valid for the project TFM
                var nearestFramework = _frameworkReducer.GetNearest(
                    projectFramework,
                    packageVersion.DependencySets.Select(group => group.TargetFramework));

                if (nearestFramework != null)
                {
                    var matches = packageVersion.DependencySets.Where(e => (e.TargetFramework.Equals(nearestFramework)));
                    deps = matches.First().Packages;
                }
            }

            var result = new SourcePackageDependencyInfo(
                identity,
                deps,
                packageVersion.IsListed,
                _source,
                new Uri(packageVersion.DownloadUrl),
                packageVersion.PackageHash);

            return result;
        }

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        private async Task<IReadOnlyList<V2FeedPackageInfo>> FindPackageById(string packageId, bool includeUnlisted, bool includePrerelease, SourceCacheContext sourceCacheContext, ILogger log, CancellationToken token)
        {
            if (await _feedCapabilities.SupportsFindPackagesByIdAsync(log, sourceCacheContext, token))
            {
                return await _feedParser.FindPackagesByIdAsync(packageId, includeUnlisted, includePrerelease, sourceCacheContext, log, token);
            }
            else
            {
                return await _feedParser.GetPackageVersionsAsync(packageId, includeUnlisted, includePrerelease, sourceCacheContext, log, token);
            }
        }

        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
    }
}
