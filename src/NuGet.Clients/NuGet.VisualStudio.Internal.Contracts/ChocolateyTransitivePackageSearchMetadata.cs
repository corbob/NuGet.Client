// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

//////////////////////////////////////////////////////////
// Chocolatey Specific Modification
//////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using NuGet.Protocol.Core.Types;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public partial class TransitivePackageSearchMetadata : IPackageSearchMetadata
    {
        public string PackageHash => _packageSearchMetadata.PackageHash;
        public string PackageHashAlgorithm => _packageSearchMetadata.PackageHashAlgorithm;
        public long? PackageSize => _packageSearchMetadata.PackageSize;
        public int? VersionDownloadCount => _packageSearchMetadata.VersionDownloadCount;
        public bool IsApproved => _packageSearchMetadata.IsApproved;
        public string PackageStatus => _packageSearchMetadata.PackageStatus;
        public string PackageSubmittedStatus => _packageSearchMetadata.PackageSubmittedStatus;
        public string PackageTestResultStatus => _packageSearchMetadata.PackageTestResultStatus;
        public DateTime? PackageTestResultStatusDate => _packageSearchMetadata.PackageTestResultStatusDate;
        public string PackageValidationResultStatus => _packageSearchMetadata.PackageValidationResultStatus;
        public DateTime? PackageValidationResultDate => _packageSearchMetadata.PackageValidationResultDate;
        public DateTime? PackageCleanupResultDate => _packageSearchMetadata.PackageCleanupResultDate;
        public DateTime? PackageReviewedDate => _packageSearchMetadata.PackageReviewedDate;
        public DateTime? PackageApprovedDate => _packageSearchMetadata.PackageApprovedDate;
        public string PackageReviewer => _packageSearchMetadata.PackageReviewer;

        public bool IsDownloadCacheAvailable => _packageSearchMetadata.IsDownloadCacheAvailable;
        public DateTime? DownloadCacheDate => _packageSearchMetadata.DownloadCacheDate;
        public IEnumerable<DownloadCache> DownloadCache => _packageSearchMetadata.DownloadCache;

        public string PackagePath => _packageSearchMetadata.PackagePath;
        public string ReleaseNotes => _packageSearchMetadata.ReleaseNotes;
        public Uri ProjectSourceUrl => _packageSearchMetadata.ProjectSourceUrl;
        public Uri PackageSourceUrl => _packageSearchMetadata.PackageSourceUrl;
        public Uri DocsUrl => _packageSearchMetadata.DocsUrl;
        public Uri MailingListUrl => _packageSearchMetadata.MailingListUrl;
        public Uri BugTrackerUrl => _packageSearchMetadata.BugTrackerUrl;
        public string DownloadCacheStatus => _packageSearchMetadata.DownloadCacheStatus;
        public string PackageScanStatus => _packageSearchMetadata.PackageScanStatus;
        public DateTime? PackageScanResultDate => _packageSearchMetadata.PackageScanResultDate;
        public string PackageScanFlagResult => _packageSearchMetadata.PackageScanFlagResult;
    }
}
