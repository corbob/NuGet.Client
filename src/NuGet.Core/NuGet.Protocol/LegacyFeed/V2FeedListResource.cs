// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    public class V2FeedListResource : ListResource
    {
        private readonly ILegacyFeedCapabilityResource _feedCapabilities;
        private readonly IV2FeedParser _feedParser;
        private readonly string _baseAddress;
        private const int Take = 30;

        public V2FeedListResource(IV2FeedParser feedParser, ILegacyFeedCapabilityResource feedCapabilities, string baseAddress)
        {
            _feedParser = feedParser;
            _feedCapabilities = feedCapabilities;
            _baseAddress = baseAddress;
        }

        public override string Source => _baseAddress;

        public async override Task<IEnumerableAsync<IPackageSearchMetadata>> ListAsync(
            string searchTerm,
            bool prerelease,
            bool allVersions,
            bool includeDelisted,
            ILogger logger,
            CancellationToken token)
        {
        //////////////////////////////////////////////////////////
        // Start - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

            return await ListAsync(searchTerm, prerelease, allVersions, includeDelisted, logger, cacheContext: null, token);
        }

        public async override Task<IEnumerableAsync<IPackageSearchMetadata>> ListAsync(
            string searchTerm,
            bool prerelease,
            bool allVersions,
            bool includeDelisted,
            ILogger logger,
            SourceCacheContext cacheContext,
            CancellationToken token)
        {
            var isSearchSupported = await _feedCapabilities.SupportsSearchAsync(logger, cacheContext, token);
        //////////////////////////////////////////////////////////
        // End - Chocolatey Specific Modification
        //////////////////////////////////////////////////////////

            SearchFilter filter = null;
            if (isSearchSupported)
            {
                if (allVersions)
                {
                    filter = new SearchFilter(includePrerelease: prerelease, filter: null)
                    {
                        OrderBy = SearchOrderBy.Version,
                        IncludeDelisted = includeDelisted
                    };
                }
                else
                {
                    //////////////////////////////////////////////////////////
                    // Start - Chocolatey Specific Modification
                    //////////////////////////////////////////////////////////

                    var supportsIsAbsoluteLatestVersion =
                        await _feedCapabilities.SupportsIsAbsoluteLatestVersionAsync(logger, cacheContext, token);

                    if (prerelease)
                    {
                        if (supportsIsAbsoluteLatestVersion)
                        {
                            filter = new SearchFilter(includePrerelease: true, filter: SearchFilterType.IsAbsoluteLatestVersion)
                            {
                                OrderBy = SearchOrderBy.Id,
                                IncludeDelisted = includeDelisted
                            };
                        }
                        else
                        {
                            filter = new SearchFilter(includePrerelease: true, filter: null)
                            {
                                OrderBy = SearchOrderBy.Version,
                                IncludeDelisted = includeDelisted
                            };
                        }

                        //////////////////////////////////////////////////////////
                        // End - Chocolatey Specific Modification
                        //////////////////////////////////////////////////////////
                    }
                    else
                    {
                        filter = new SearchFilter(includePrerelease: false,
                            filter: SearchFilterType.IsLatestVersion)
                        {
                            OrderBy = SearchOrderBy.Id,
                            IncludeDelisted = includeDelisted
                        };
                    }
                }
            }
            else
            {
                if (allVersions)
                {
                    filter = new SearchFilter(includePrerelease: prerelease, filter: null)
                    {
                        IncludeDelisted = includeDelisted,
                        OrderBy = SearchOrderBy.Version
                    };
                }
                else
                {
                    //////////////////////////////////////////////////////////
                    // Start - Chocolatey Specific Modification
                    //////////////////////////////////////////////////////////

                    var supportsIsAbsoluteLatestVersion =
                        await _feedCapabilities.SupportsIsAbsoluteLatestVersionAsync(logger, cacheContext, token);

                    if (prerelease)
                    {
                        if (supportsIsAbsoluteLatestVersion)
                        {
                            filter = new SearchFilter(includePrerelease: true, filter: SearchFilterType.IsAbsoluteLatestVersion)
                            {
                                IncludeDelisted = includeDelisted,
                                OrderBy = SearchOrderBy.Id
                            };
                        }
                        else
                        {
                            filter = new SearchFilter(includePrerelease: true, null)
                            {
                                IncludeDelisted = includeDelisted,
                                OrderBy = SearchOrderBy.Version
                            };
                        }

                        //////////////////////////////////////////////////////////
                        // End - Chocolatey Specific Modification
                        //////////////////////////////////////////////////////////
                    }
                    else
                    {
                        filter = new SearchFilter(includePrerelease: false,
                            filter: SearchFilterType.IsLatestVersion)
                        {
                            OrderBy = SearchOrderBy.Id,
                            IncludeDelisted = includeDelisted
                        };
                    }
                }

            }

            //////////////////////////////////////////////////////////
            // Start - Chocolatey Specific Modification
            //////////////////////////////////////////////////////////
            return new EnumerableAsync<IPackageSearchMetadata>(
                _feedParser,
                searchTerm,
                filter,
                0,
                Take,
                isSearchSupported,
                allVersions,
                logger,
                cacheContext,
                token);
            //////////////////////////////////////////////////////////
            // End - Chocolatey Specific Modification
            //////////////////////////////////////////////////////////
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
            var metadataCache = new MetadataReferenceCache();

            //////////////////////////////////////////////////////////
            // Start - Chocolatey Specific Modification
            //////////////////////////////////////////////////////////

            var supportsIsAbsoluteLatestVersion = await _feedCapabilities.SupportsIsAbsoluteLatestVersionAsync(logger, cacheContext, token);

            //////////////////////////////////////////////////////////
            // End - Chocolatey Specific Modification
            //////////////////////////////////////////////////////////

            SearchFilter searchFilter = null;

            if (prerelease)
            {
                if (supportsIsAbsoluteLatestVersion)
                {
                    searchFilter = new SearchFilter(includePrerelease: true, filter: SearchFilterType.IsAbsoluteLatestVersion);
                }
                else
                {
                    // In this scenario, we can't do a IsLatestVersion or IsAbsoluteVersion query, which means that any query we
                    // do will potentially result in more than one result, which is not desired.
                    // Instead, use the FindPackageByIdAsync method, and then order the results by version, and return the
                    // top result.
                    //////////////////////////////////////////////////////////
                    // Start - Chocolatey Specific Modification
                    //////////////////////////////////////////////////////////
                    var packages = await FindPackageByIdAsync(searchTerm, includeUnlisted: false, prerelease, sourceCacheContext: cacheContext, logger, token);
                    //////////////////////////////////////////////////////////
                    // End - Chocolatey Specific Modification
                    //////////////////////////////////////////////////////////

                    searchFilter = new SearchFilter(includePrerelease: true, filter: null);
                    searchFilter.OrderBy = SearchOrderBy.Version;

                    if (packages.Count == 0)
                    {
                        return null;
                    }

                    var highestVersionPackage = packages.OrderByDescending(p => p.Version).FirstOrDefault();
                    return V2FeedUtilities.CreatePackageSearchResult(highestVersionPackage, metadataCache, searchFilter, (V2FeedParser)_feedParser, logger, token);
                }
            }
            else
            {
                searchFilter = new SearchFilter(includePrerelease: false, filter: SearchFilterType.IsLatestVersion);
            }

            searchFilter.ExactPackageId = true;

            //////////////////////////////////////////////////////////
            // Start - Chocolatey Specific Modification
            //////////////////////////////////////////////////////////
            var pageOfResults = await _feedParser.GetPackagesPageAsync(searchTerm, searchFilter, 0, 1, logger, cacheContext, token);
            //////////////////////////////////////////////////////////
            // End - Chocolatey Specific Modification
            //////////////////////////////////////////////////////////

            if (!pageOfResults.Items.Any())
            {
                return null;
            }

            return V2FeedUtilities.CreatePackageSearchResult(pageOfResults.Items[0], metadataCache, searchFilter, (V2FeedParser)_feedParser, logger, token);
        }

        private async Task<IReadOnlyList<V2FeedPackageInfo>> FindPackageByIdAsync(string packageId, bool includeUnlisted, bool includePrerelease, SourceCacheContext sourceCacheContext, Common.ILogger log, CancellationToken token)
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

internal class EnumerableAsync<T> : IEnumerableAsync<T>
{
    private readonly SearchFilter _filter;
    private readonly ILogger _logger;
    private readonly string _searchTerm;
    private readonly int _skip;
    private readonly int _take;
    private readonly CancellationToken _token;
    private readonly IV2FeedParser _feedParser;
    private readonly bool _isSearchAvailable;
    private readonly bool _allVersions;

    //////////////////////////////////////////////////////////
    // Start - Chocolatey Specific Modification
    //////////////////////////////////////////////////////////

    private readonly SourceCacheContext _cacheContext;
    

    public EnumerableAsync(IV2FeedParser feedParser, string searchTerm, SearchFilter filter, int skip, int take, bool isSearchAvailable, bool allVersions, ILogger logger, CancellationToken token)
        : this(feedParser, searchTerm, filter, skip, take, isSearchAvailable, allVersions, logger, cacheContext: null, token)
    {

    }

    public EnumerableAsync(IV2FeedParser feedParser, string searchTerm, SearchFilter filter, int skip, int take, bool isSearchAvailable, bool allVersions, ILogger logger, SourceCacheContext cacheContext, CancellationToken token)
    {
        _feedParser = feedParser;
        _searchTerm = searchTerm;
        _filter = filter;
        _skip = skip;
        _take = take;
        _isSearchAvailable = isSearchAvailable;
        _allVersions = allVersions;
        _logger = logger;
        _cacheContext = cacheContext;
        _token = token;
    }

    //////////////////////////////////////////////////////////
    // End - Chocolatey Specific Modification
    //////////////////////////////////////////////////////////

    public IEnumeratorAsync<T> GetEnumeratorAsync()
    {
        return (IEnumeratorAsync<T>)new EnumeratorAsync(_feedParser, _searchTerm, _filter, _skip, _take, _isSearchAvailable, _allVersions, _logger, _cacheContext, _token);
    }
}

internal class EnumeratorAsync : IEnumeratorAsync<IPackageSearchMetadata>
{
    private readonly SearchFilter _filter;
    private readonly ILogger _logger;
    private readonly string _searchTerm;
    private int _skip;
    private readonly int _take;
    private readonly CancellationToken _token;
    private readonly IV2FeedParser _feedParser;
    private readonly bool _isSearchAvailable;
    private readonly bool _allVersions;


    private IEnumerator<IPackageSearchMetadata> _currentEnumerator;
    private V2FeedPage _currentPage;

    //////////////////////////////////////////////////////////
    // Start - Chocolatey Specific Modification
    //////////////////////////////////////////////////////////            
    
    private readonly SourceCacheContext _cacheContext;
    
    public EnumeratorAsync(IV2FeedParser feedParser, string searchTerm, SearchFilter filter, int skip, int take, bool isSearchAvailable, bool allVersions,
        ILogger logger, CancellationToken token)
        : this(feedParser, searchTerm, filter, skip, take, isSearchAvailable, allVersions, logger, cacheContext: null, token)
    {

    }

    public EnumeratorAsync(IV2FeedParser feedParser, string searchTerm, SearchFilter filter, int skip, int take, bool isSearchAvailable, bool allVersions,
        ILogger logger, SourceCacheContext cacheContext, CancellationToken token)
    {
        _feedParser = feedParser;
        _searchTerm = searchTerm;
        _filter = filter;
        _skip = skip;
        _take = take;
        _isSearchAvailable = isSearchAvailable;
        _allVersions = allVersions;
        _logger = logger;
        _cacheContext = cacheContext;
        _token = token;
    }
    //////////////////////////////////////////////////////////
    // Start - Chocolatey Specific Modification
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
        var metadataCache = new MetadataReferenceCache();

        if (_currentPage == null)
        {

            //////////////////////////////////////////////////////////
            // Start - Chocolatey Specific Modification
            //////////////////////////////////////////////////////////  
            _currentPage = _isSearchAvailable
                ? await _feedParser.GetSearchPageAsync(_searchTerm, _filter, _skip, _take, _logger, _cacheContext, _token)
                : await _feedParser.GetPackagesPageAsync(_searchTerm, _filter, _skip, _take, _logger, _cacheContext, _token);
            //////////////////////////////////////////////////////////
            // End - Chocolatey Specific Modification
            //////////////////////////////////////////////////////////
                

            var results = _allVersions ?
                _currentPage.Items.GroupBy(p => p.Id)
                 .Select(group => group.OrderByDescending(p => p.Version)).SelectMany(pg => pg)
                 .Select(
                     package =>
                         V2FeedUtilities.CreatePackageSearchResult(package, metadataCache, _filter,
                             (V2FeedParser)_feedParser, _logger, _token)).Where(p => _filter.IncludeDelisted || p.IsListed)
            :
            _currentPage.Items.GroupBy(p => p.Id)
                 .Select(group => group.OrderByDescending(p => p.Version).First())
                 .Select(
                     package =>
                         V2FeedUtilities.CreatePackageSearchResult(package, metadataCache, _filter,
                             (V2FeedParser)_feedParser, _logger, _token)).Where(p => _filter.IncludeDelisted || p.IsListed);


            var enumerator = results.GetEnumerator();
            _currentEnumerator = enumerator;
            return _currentEnumerator.MoveNext();
        }
        else
        {
            if (!_currentEnumerator.MoveNext())
            {
                if (_currentPage.Items.Count != _take) // Last page not filled completely, no more pages left
                {
                    return false;
                }
                _skip += _take;
                //////////////////////////////////////////////////////////
                // Start - Chocolatey Specific Modification
                //////////////////////////////////////////////////////////
                _currentPage = _isSearchAvailable
                                    ? await _feedParser.GetSearchPageAsync(_searchTerm, _filter, _skip, _take, _logger, _cacheContext, _token)
                                    : await _feedParser.GetPackagesPageAsync(_searchTerm, _filter, _skip, _take, _logger, _cacheContext, _token);
                //////////////////////////////////////////////////////////
                // End - Chocolatey Specific Modification
                //////////////////////////////////////////////////////////
                
                var results = _allVersions ?
               _currentPage.Items.GroupBy(p => p.Id)
                .Select(group => group.OrderByDescending(p => p.Version)).SelectMany(pg => pg)
                .Select(
                    package =>
                        V2FeedUtilities.CreatePackageSearchResult(package, metadataCache, _filter,
                            (V2FeedParser)_feedParser, _logger, _token)).Where(p => _filter.IncludeDelisted || p.IsListed)
                :
                _currentPage.Items.GroupBy(p => p.Id)
                 .Select(group => group.OrderByDescending(p => p.Version).First())
                 .Select(
                     package =>
                         V2FeedUtilities.CreatePackageSearchResult(package, metadataCache, _filter,
                             (V2FeedParser)_feedParser, _logger, _token)).Where(p => _filter.IncludeDelisted || p.IsListed);

                var enumerator = results.GetEnumerator();
                _currentEnumerator = enumerator;
                return _currentEnumerator.MoveNext();
            }
            else
            {
                return true;
            }

        }
    }
}
