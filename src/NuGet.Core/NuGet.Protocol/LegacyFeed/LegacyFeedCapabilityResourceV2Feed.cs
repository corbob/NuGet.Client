// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Common;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    /// <summary>
    /// A resource for detecting a V2 feed's capabilities based on the metadata document.
    /// </summary>
    public class LegacyFeedCapabilityResourceV2Feed : LegacyFeedCapabilityResource
    {
        private static readonly ConcurrentDictionary<string, Task<Capabilities>> CachedCapabilities
            = new ConcurrentDictionary<string, Task<Capabilities>>();

        private const string MetadataUriFormat = "{0}/$metadata";

        private readonly string _metadataUri;
        private readonly V2FeedParser _feedParser;

        public LegacyFeedCapabilityResourceV2Feed(V2FeedParser feedParser, string baseAddress)
        {
            _feedParser = feedParser;
            _metadataUri = string.Format(
                CultureInfo.InvariantCulture,
                MetadataUriFormat,
                baseAddress);
        }

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        public override async Task<bool> SupportsIsAbsoluteLatestVersionAsync(ILogger log, CancellationToken token)
        {
            return await SupportsIsAbsoluteLatestVersionAsync(log, cacheContext: null, token);
        }

        public override async Task<bool> SupportsIsAbsoluteLatestVersionAsync(ILogger log, SourceCacheContext cacheContext, CancellationToken token)
        {
            var capabilities = await GetCachedCapabilitiesAsync(log, cacheContext, token);

            return capabilities.SupportsIsAbsoluteLatestVersion;
        }

        public override async Task<bool> SupportsSearchAsync(ILogger log, CancellationToken token)
        {
            return await SupportsSearchAsync(log, cacheContext: null, token);
        }

        public override async Task<bool> SupportsSearchAsync(ILogger log, SourceCacheContext cacheContext, CancellationToken token)
        {
            var capabilities = await GetCachedCapabilitiesAsync(log, cacheContext, token);

            return capabilities.SupportsSearch;
        }

        public override async Task<bool> SupportsFindPackagesByIdAsync(ILogger log, CancellationToken token)
        {
            return await SupportsFindPackagesByIdAsync(log, cacheContext: null, token);
        }

        public override async Task<bool> SupportsFindPackagesByIdAsync(ILogger log, SourceCacheContext cacheContext, CancellationToken token)
        {
            var capabilities = await GetCachedCapabilitiesAsync(log, cacheContext, token);

            return capabilities.SupportsFindPackageById;
        }

        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        private async Task<Capabilities> GetCachedCapabilitiesAsync(ILogger log, SourceCacheContext cacheContext, CancellationToken token)
        {
            var task = CachedCapabilities.GetOrAdd(
                _metadataUri,
                key => GetCapabilitiesAsync(key, log, cacheContext, token));

            return await task;
        }

        private async Task<Capabilities> GetCapabilitiesAsync(string metadataUri, ILogger log, SourceCacheContext cacheContext, CancellationToken token)
        {
            var capabilities = new Capabilities
            {
                SupportsIsAbsoluteLatestVersion = true,
                SupportsSearch = true
            };

            XDocument document;
            try
            {
                //////////////////////////////////////////////////////////
                // Start - Chocolatey Specific Modification
                //////////////////////////////////////////////////////////

                document = await _feedParser.LoadXmlAsync(
                    metadataUri,
                    cacheKey: "$metadata",
                    ignoreNotFounds: true,
                    sourceCacheContext: cacheContext,
                    log: log,
                    token: token);
                //////////////////////////////////////////////////////////
                // End - Chocolatey Specific Modification
                //////////////////////////////////////////////////////////

                var metadata = DataServiceMetadataExtractor.Extract(document);

                capabilities.SupportsIsAbsoluteLatestVersion = metadata
                    .SupportedProperties
                    .Contains("IsAbsoluteLatestVersion");

                capabilities.SupportsSearch = metadata
                    .SupportedMethodNames
                    .Contains("Search");

                //////////////////////////////////////////////////////////
                // Start - Chocolatey Specific Modification
                //////////////////////////////////////////////////////////

                capabilities.SupportsFindPackageById = metadata
                    .SupportedMethodNames
                    .Contains("FindPackagesById");

                capabilities.SupportsGetUpdates = metadata
                    .SupportedMethodNames
                    .Contains("GetUpdates");

                //////////////////////////////////////////////////////////
                // End - Chocolatey Specific Modification
                //////////////////////////////////////////////////////////
            }
            catch
            {
                // If there is a failure getting the metadata, assume default capabilities.

                //////////////////////////////////////////////////////////
                // Start - Chocolatey Specific Modification
                //////////////////////////////////////////////////////////

                // If we are unable to grab information directly from the metadata URL for a source
                // regarding what capabilities that it has, let's assume that it can't do either of
                // the following.  This is in-keeping with what was done in the original NuGet.Core
                // assembly.
                capabilities.SupportsIsAbsoluteLatestVersion = false;
                capabilities.SupportsSearch = false;

                //////////////////////////////////////////////////////////
                // End - Chocolatey Specific Modification
                //////////////////////////////////////////////////////////
            }

            return capabilities;
        }

        private class Capabilities
        {
            public string MetadataUri { get; set; }

            //////////////////////////////////////////////////////////
            // Start - Chocolatey Specific Modification
            //////////////////////////////////////////////////////////

            public bool SupportsFindPackageById { get; set; }
            public bool SupportsGetUpdates { get; set; }

            //////////////////////////////////////////////////////////
            // End - Chocolatey Specific Modification
            //////////////////////////////////////////////////////////

            public bool SupportsIsAbsoluteLatestVersion { get; set; }
            public bool SupportsSearch { get; set; }
        }

        private class DataServiceMetadata
        {
            public HashSet<string> SupportedMethodNames { get; set; }

            public HashSet<string> SupportedProperties { get; set; }
        }

        /// <summary>
        /// This implementation is copied from NuGet 2.x.
        /// </summary>
        private static class DataServiceMetadataExtractor
        {
            public static DataServiceMetadata Extract(XDocument schemaDocument)
            {
                // Get all entity containers
                var entityContainers = from e in schemaDocument.Descendants()
                                       where e.Name.LocalName == "EntityContainer"
                                       select e;

                // Find the entity container with the Packages entity set
                var result = (from e in entityContainers
                              let entitySet = e.Elements().FirstOrDefault(el => el.Name.LocalName == "EntitySet")
                              let name = entitySet != null ? entitySet.Attribute("Name").Value : null
                              where name != null && name.Equals("Packages", StringComparison.OrdinalIgnoreCase)
                              select new { Container = e, EntitySet = entitySet }).FirstOrDefault();

                if (result == null)
                {
                    return null;
                }

                var packageEntityContainer = result.Container;
                var packageEntityTypeAttribute = result.EntitySet.Attribute("EntityType");

                string packageEntityName = null;
                if (packageEntityTypeAttribute != null)
                {
                    packageEntityName = packageEntityTypeAttribute.Value;
                }

                var methodNames =
                    from e in packageEntityContainer.Elements()
                    where e.Name.LocalName == "FunctionImport"
                    select e.Attribute("Name").Value;

                var metadata = new DataServiceMetadata
                {
                    SupportedMethodNames = new HashSet<string>(
                        methodNames,
                        StringComparer.OrdinalIgnoreCase),

                    SupportedProperties = new HashSet<string>(
                        ExtractSupportedProperties(schemaDocument, packageEntityName),
                        StringComparer.OrdinalIgnoreCase)
                };

                return metadata;
            }

            private static IEnumerable<string> ExtractSupportedProperties(
                XDocument schemaDocument,
                string packageEntityName)
            {
                packageEntityName = TrimNamespace(packageEntityName);

                var packageEntity = (from e in schemaDocument.Descendants()
                                     where e.Name.LocalName == "EntityType"
                                     let attribute = e.Attribute("Name")
                                     where attribute != null && attribute.Value.Equals(
                                         packageEntityName,
                                         StringComparison.OrdinalIgnoreCase)
                                     select e).FirstOrDefault();

                if (packageEntity != null)
                {
                    return from e in packageEntity.Elements()
                           where e.Name.LocalName == "Property"
                           select e.Attribute("Name").Value;
                }

                return Enumerable.Empty<string>();
            }

            private static string TrimNamespace(string packageEntityName)
            {
                var lastIndex = packageEntityName.LastIndexOf('.');
                if (lastIndex > 0 && lastIndex < packageEntityName.Length)
                {
                    packageEntityName = packageEntityName.Substring(lastIndex + 1);
                }
                return packageEntityName;
            }
        }
    }
}
