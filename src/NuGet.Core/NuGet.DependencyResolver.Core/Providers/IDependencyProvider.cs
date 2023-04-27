// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
using Chocolatey.NuGet.Frameworks;
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
using NuGet.LibraryModel;

namespace NuGet.DependencyResolver
{
    public interface IDependencyProvider
    {
        bool SupportsType(LibraryDependencyTarget libraryTypeFlag);

        Library GetLibrary(LibraryRange libraryRange, NuGetFramework targetFramework);
    }
}
