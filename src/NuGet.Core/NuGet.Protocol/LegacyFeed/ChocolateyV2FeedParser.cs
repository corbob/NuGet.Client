// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

//////////////////////////////////////////////////////////
// Chocolatey Specific Modification
//////////////////////////////////////////////////////////

using System;
using System.Xml.Linq;

namespace NuGet.Protocol
{
    public sealed partial class V2FeedParser : IV2FeedParser
    {
#pragma warning disable IDE1006 // Naming Styles
        private static readonly XName _xnamePackageSize = XName.Get("PackageSize", DataServicesNS);
        private static readonly XName _xnameVersionDownloadCount = XName.Get("VersionDownloadCount", DataServicesNS);
        private static readonly XName _xnameIsApproved = XName.Get("IsApproved", DataServicesNS);
        private static readonly XName _xnamePackageStatus = XName.Get("PackageStatus", DataServicesNS);
        private static readonly XName _xnamePackageSubmittedStatus = XName.Get("PackageSubmittedStatus", DataServicesNS);
        private static readonly XName _xnamePackageTestResultStatus = XName.Get("PackageTestResultStatus", DataServicesNS);
        private static readonly XName _xnamePackageTestResultStatusDate = XName.Get("PackageTestResultStatusDate", DataServicesNS);
        private static readonly XName _xnamePackageValidationResultStatus = XName.Get("PackageValidationResultStatus", DataServicesNS);
        private static readonly XName _xnamePackageValidationResultDate = XName.Get("PackageValidationResultDate", DataServicesNS);
        private static readonly XName _xnamePackageCleanupResultDate = XName.Get("PackageCleanupResultDate", DataServicesNS);
        private static readonly XName _xnamePackageReviewedDate = XName.Get("PackageReviewedDate", DataServicesNS);
        private static readonly XName _xnamePackageApprovedDate = XName.Get("PackageApprovedDate", DataServicesNS);
        private static readonly XName _xnamePackageReviewer = XName.Get("PackageReviewer", DataServicesNS);
        private static readonly XName _xnameIsDownloadCacheAvailable = XName.Get("IsDownloadCacheAvailable", DataServicesNS);
        private static readonly XName _xnameDownloadCacheDate = XName.Get("DownloadCacheDate", DataServicesNS);
        private static readonly XName _xnameDownloadCache = XName.Get("DownloadCache", DataServicesNS);
        private static readonly XName _xnameIsLatestVersion = XName.Get("IsLatestVersion", DataServicesNS);
        private static readonly XName _xnameIsAbsoluteLatestVersion = XName.Get("IsAbsoluteLatestVersion", DataServicesNS);
        private static readonly XName _xnameIsPrerelease = XName.Get("IsPrerelease", DataServicesNS);
        private static readonly XName _xnameReleaseNotes = XName.Get("ReleaseNotes", DataServicesNS);
        private static readonly XName _xnameProjectSourceUrl = XName.Get("ProjectSourceUrl", DataServicesNS);
        private static readonly XName _xnamePackageSourceUrl = XName.Get("PackageSourceUrl", DataServicesNS);
        private static readonly XName _xnameDocsUrl = XName.Get("DocsUrl", DataServicesNS);
        private static readonly XName _xnameMailingListUrl = XName.Get("MailingListUrl", DataServicesNS);
        private static readonly XName _xnameBugTrackerUrl = XName.Get("BugTrackerUrl", DataServicesNS);
        private static readonly XName _xnameDownloadCacheStatus = XName.Get("DownloadCacheStatus", DataServicesNS);
        private static readonly XName _xnamePackageScanStatus = XName.Get("PackageScanStatus", DataServicesNS);
        private static readonly XName _xnamePackageScanResultDate = XName.Get("PackageScanResultDate", DataServicesNS);
        private static readonly XName _xnamePackageScanFlagResult = XName.Get("PackageScanFlagResult", DataServicesNS);
#pragma warning restore IDE1006 // Naming Styles


        /// <summary>
        /// Retrieve an XML <see cref="DateTime"/> value safely
        /// </summary>
        private static DateTime? GetNoOffsetDate(XElement parent, XName childName)
        {
            var dateString = GetString(parent, childName);

            DateTime date;
            if (DateTime.TryParse(dateString, out date))
            {
                return date;
            }

            return null;
        }

        /// <summary>
        /// Retrieve an XML <see cref="long"/> value safely
        /// </summary>
        private static long? GetLong(XElement parent, XName childName)
        {
            var numberString = GetString(parent, childName);

            long number;
            if (long.TryParse(numberString, out number))
            {
                return number;
            }

            return null;
        }

        /// <summary>
        /// Retrieve an XML <see cref="int"/> value safely
        /// </summary>
        private static int? GetInt(XElement parent, XName childName)
        {
            var numberString = GetString(parent, childName);

            int number;
            if (int.TryParse(numberString, out number))
            {
                return number;
            }

            return null;
        }
    }
}
