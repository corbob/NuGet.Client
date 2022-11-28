// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
using Chocolatey.NuGet.Frameworks;
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
using Xunit;

namespace NuGet.VisualStudio.Internal.Contracts.Test
{
    public sealed class NuGetFrameworkFormatterTests : FormatterTests
    {
        [Theory]
        [MemberData(nameof(NuGetFrameworks))]
        public void SerializeThenDeserialize_WithValidArguments_RoundTrips(NuGetFramework expectedResult)
        {
            NuGetFramework? actualResult = SerializeThenDeserialize(NuGetFrameworkFormatter.Instance, expectedResult);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, actualResult);
        }

        public static TheoryData NuGetFrameworks => new TheoryData<NuGetFramework>
            {
                { new NuGetFramework(FrameworkConstants.FrameworkIdentifiers.Net, new Version(4, 5), "Profile344") },
                { new NuGetFramework(FrameworkConstants.FrameworkIdentifiers.Net, new Version(4, 8)) },
                { FrameworkConstants.CommonFrameworks.Net50 }
            };
    }
}
