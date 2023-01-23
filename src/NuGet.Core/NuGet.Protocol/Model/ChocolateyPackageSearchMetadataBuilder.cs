// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

//////////////////////////////////////////////////////////
// Chocolatey Specific Modification
//////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace NuGet.Protocol.Core.Types
{
    public partial class PackageSearchMetadataBuilder
    {
        public partial class ClonedPackageSearchMetadata : IPackageSearchMetadata
        {
            public string PackageHash { get; set; }
            public string PackageHashAlgorithm { get; set; }
            public long? PackageSize { get; set; }
            public int? VersionDownloadCount { get; set; }

            public bool IsApproved { get; set; }
            public string PackageStatus { get; set; }
            public string PackageSubmittedStatus { get; set; }
            public string PackageTestResultStatus { get; set; }
            public DateTime? PackageTestResultStatusDate { get; set; }
            public string PackageValidationResultStatus { get; set; }
            public DateTime? PackageValidationResultDate { get; set; }
            public DateTime? PackageCleanupResultDate { get; set; }
            public DateTime? PackageReviewedDate { get; set; }
            public DateTime? PackageApprovedDate { get; set; }
            public string PackageReviewer { get; set; }

            public bool IsDownloadCacheAvailable { get; set; }
            public DateTime? DownloadCacheDate { get; set; }
            public IEnumerable<DownloadCache> DownloadCache { get; set; }
            public bool IsLatestVersion { get; set; }
            public bool IsAbsoluteLatestVersion { get; set; }
            public bool IsPrerelease { get; set; }
            public string ReleaseNotes { get; set; }
            public Uri ProjectSourceUrl { get; set; }
            public Uri PackageSourceUrl { get; set; }
            public Uri DocsUrl { get; set; }
            public Uri MailingListUrl { get; set; }
            public Uri BugTrackerUrl { get; set; }
            public string DownloadCacheStatus { get; set; }
            public string PackageScanStatus { get; set; }
            public DateTime? PackageScanResultDate { get; set; }
            public string PackageScanFlagResult { get; set; }
        }
    }
}
