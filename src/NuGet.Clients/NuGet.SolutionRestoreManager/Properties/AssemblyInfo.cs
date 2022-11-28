// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("NuGet.SolutionRestoreManager")]
[assembly: AssemblyDescription("NuGet Solution Restore Manager")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("06662133-1292-4918-90f3-36c930c0b16f")]

[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Lucene.Net.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Newtonsoft.Json.dll")]
//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.Commands.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.Common.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.Configuration.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.Credentials.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.DependencyResolver.Core.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.Frameworks.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.Indexing.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.LibraryModel.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.PackageManagement.dll")]
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.PackageManagement.VisualStudio.dll")]
//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.Packaging.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.ProjectModel.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.Protocol.dll")]
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.Resolver.dll")]
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.SolutionRestoreManager.dll")]
//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.Versioning.dll")]
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.VisualStudio.Common.dll")]
//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.VisualStudio.dll")]
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
[assembly: ProvideCodeBase(CodeBase = @"$PackageFolder$\NuGet.VisualStudio.Internal.Contracts.dll")]

//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
[assembly: ProvideBindingRedirection(CodeBase = @"$PackageFolder$\Chocolatey.NuGet.VisualStudio.dll", OldVersionLowerBound = "0.0.0.0")]
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
[assembly: InternalsVisibleTo("NuGet.SolutionRestoreManager.Test, PublicKey=002400000480000094000000060200000024000052534131000400000100010007d1fa57c4aed9f0a32e84aa0faefd0de9e8fd6aec8f87fb03766c834c99921eb23be79ad9d5dcc1dd9ad236132102900b723cf980957fc4e177108fc607774f29e8320e92ea05ece4e821c0a5efe8f1645c4c0c93c1ab99285d622caa652c1dfad63d745d6f2de5f17e5eaf0fc4963d261c8a12436518206dc093344d5ad293")]
