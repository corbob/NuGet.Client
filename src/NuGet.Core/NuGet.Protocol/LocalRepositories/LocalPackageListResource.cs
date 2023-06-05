// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class LocalPackageListResource : ListResource
    {
        private readonly PackageSearchResource _localPackageSearchResource;
        private readonly string _baseAddress;
        public LocalPackageListResource(PackageSearchResource localPackageSearchResource, string baseAddress)
        {
            _localPackageSearchResource = localPackageSearchResource;
            _baseAddress = baseAddress;
        }
        public override string Source => _baseAddress;

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
        public override async Task<IEnumerableAsync<IPackageSearchMetadata>> ListAsync(
            string searchTerm,
            bool prerelease,
            bool allVersions,
            bool includeDelisted,
            ILogger logger,
            CancellationToken token)
        {
            return await ListAsync(searchTerm, prerelease, allVersions, includeDelisted, logger, cacheContext: null, token);
        }

        public override Task<IEnumerableAsync<IPackageSearchMetadata>> ListAsync(
            string searchTerm,
            bool prerelease,
            bool allVersions,
            bool includeDelisted,
            ILogger logger,
            SourceCacheContext cacheContext,
            CancellationToken token)
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
        {
            SearchFilter filter;

            if (allVersions)
            {
                filter = new SearchFilter(includePrerelease: prerelease, filter: null)
                {
                    OrderBy = SearchOrderBy.Version,
                    IncludeDelisted = includeDelisted
                };
            }
            else if (prerelease)
            {
                filter = new SearchFilter(includePrerelease: true, filter: SearchFilterType.IsAbsoluteLatestVersion)
                {
                    OrderBy = SearchOrderBy.Id,
                    IncludeDelisted = includeDelisted
                };
            }
            else
            {
                filter = new SearchFilter(includePrerelease: false, filter: SearchFilterType.IsLatestVersion)
                {
                    OrderBy = SearchOrderBy.Id,
                    IncludeDelisted = includeDelisted
                };
            }
        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
            IEnumerableAsync<IPackageSearchMetadata> enumerable = new EnumerableAsync<IPackageSearchMetadata>(
                _localPackageSearchResource,
                searchTerm,
                filter,
                logger,
                cacheContext,
                token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
           return Task.FromResult(enumerable);

        }

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        public async override Task<IPackageSearchMetadata> PackageAsync(
            string searchTerm,
            bool prerelease,
            ILogger logger,
            CancellationToken token)
        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
        {
            return await PackageAsync(searchTerm, prerelease, logger, cacheContext: null, token);
        }

        public async override Task<IPackageSearchMetadata> PackageAsync(
            string searchTerm,
            bool prerelease,
            ILogger logger,
            SourceCacheContext cacheContext,
            CancellationToken token)
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
        {
            var searchFilter = new SearchFilter(prerelease);
            searchFilter.ExactPackageId = true;
        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

            var result = await _localPackageSearchResource.SearchAsync(searchTerm, searchFilter, 0, int.MaxValue, logger, cacheContext, token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

            return result.FirstOrDefault();
        }

        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

        internal class EnumerableAsync<T> : IEnumerableAsync<T>
        {
            private readonly SearchFilter _filter;
            private readonly ILogger _logger;
            private readonly string _searchTerm;
            private readonly CancellationToken _token;
            private readonly PackageSearchResource _packageSearchResource;

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
            private readonly SourceCacheContext _cacheContext;
            
            public EnumerableAsync(PackageSearchResource feedParser, string searchTerm, SearchFilter filter, ILogger logger, SourceCacheContext cacheContext, CancellationToken token)
            {
                _packageSearchResource = feedParser;
                _searchTerm = searchTerm;
                _filter = filter;
                _logger = logger;
                _cacheContext = cacheContext;
                _token = token;
            }
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

            public IEnumeratorAsync<T> GetEnumeratorAsync()
            {
        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
                return (IEnumeratorAsync<T>)new EnumeratorAsync(_packageSearchResource, _searchTerm, _filter, _logger, _cacheContext, _token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
            }
        }

        internal class EnumeratorAsync : IEnumeratorAsync<IPackageSearchMetadata>
        {
            private readonly SearchFilter _filter;
            private readonly ILogger _logger;
            private readonly string _searchTerm;
            private readonly CancellationToken _token;
            private readonly PackageSearchResource _packageSearchResource;


            private IEnumerator<IPackageSearchMetadata> _currentEnumerator;

        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
            private readonly SourceCacheContext _cacheContext;
            
            public EnumeratorAsync(PackageSearchResource feedParser, string searchTerm, SearchFilter filter, ILogger logger, SourceCacheContext cacheContext, CancellationToken token)
            {
                _packageSearchResource = feedParser;
                _searchTerm = searchTerm;
                _filter = filter;
                _logger = logger;
                _cacheContext = cacheContext;
                _token = token;
            }
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

            public IPackageSearchMetadata Current
            {
                get
                {
                    return _currentEnumerator?.Current;
                }
            }

            public async Task<bool> MoveNextAsync()
            {
                if (_currentEnumerator == null)
                { // NOTE: We need to sort the values so this is very innefficient by design.
                  // The FS search resource would return the results ordered in FS nat ordering.
        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
                    var results = await _packageSearchResource.SearchAsync(
                        _searchTerm, _filter, 0, int.MaxValue, _logger, _cacheContext, _token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////
                    switch (_filter.OrderBy)
                    {
                        case SearchOrderBy.Id:
                            _currentEnumerator = results.OrderBy(p => p.Identity).GetEnumerator();
                            break;

                        //////////////////////////////////////////////////////////
                        // Start - Chocolatey Specific Modification
                        //////////////////////////////////////////////////////////

                        case SearchOrderBy.DownloadCount:
                            // Local packages do not have downloads available
                            goto default;

                        case SearchOrderBy.Version:
                        case SearchOrderBy.DownloadCountAndVersion:
                            _currentEnumerator = results.OrderBy(p => p.Identity.Id).ThenByDescending(p => p.Identity.Version).GetEnumerator();
                            break;

                        //////////////////////////////////////////////////////////
                        // End - Chocolatey Specific Modification
                        //////////////////////////////////////////////////////////

                        default:
                            _currentEnumerator = results.GetEnumerator();
                            break;
                    }
                }
                return _currentEnumerator.MoveNext();
            }
        }
    }
}
