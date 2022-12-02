// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using FluentAssertions;
using Xunit;
//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
using static Chocolatey.NuGet.Frameworks.FrameworkConstants;
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
namespace Chocolatey.NuGet.Frameworks.Test
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
{
    public class DualCompatibilityFrameworkTests
    {
        [Fact]
        public void Constructor_WithNullFramework_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new DualCompatibilityFramework(framework: null, secondaryFramework: NuGetFramework.AnyFramework));
        }

        [Fact]
        public void Constructor_WithNullSecondary_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new DualCompatibilityFramework(framework: NuGetFramework.AnyFramework, secondaryFramework: null));
        }

        [Theory]
        [InlineData("net5.0", "net5.0", true)]
        [InlineData("net45", "net45", true)]
        [InlineData("net5.0-windows10.0.6508.1", "net5.0-windows10.0.6508.1", true)]
        [InlineData("net5.0-windows10.0.6508.1", "net5.0-windows10.0.6508.2", false)]
        public void NuGetFrameworkEquals_WithDualCompatibilityFramework_Succeeds(string shortFrameworkName, string rootFrameworkName, bool equals)
        {
            var nugetFramework = NuGetFramework.Parse(shortFrameworkName);
            var extendedFramework = new DualCompatibilityFramework(NuGetFramework.Parse(rootFrameworkName), secondaryFramework: NuGetFramework.Parse(rootFrameworkName));
            var comparer = new NuGetFrameworkFullComparer();
            comparer.Equals(nugetFramework, extendedFramework).Should().Be(equals);
            nugetFramework.Equals(extendedFramework).Should().Be(equals);
        }

        [Theory]
        [InlineData("net5.0")]
        [InlineData("net45")]
        [InlineData("net5.0-windows10.0.6508.1")]
        public void DualCompatibilityFrameworkEquals_WithNonDualCompatibilityFramework_Succeeds(string shortFrameworkName)
        {
            var nugetFramework = NuGetFramework.Parse(shortFrameworkName);
            var dualCompatibilityFramework = new DualCompatibilityFramework(nugetFramework, secondaryFramework: NuGetFramework.AnyFramework);
            Assert.False(dualCompatibilityFramework.Equals((object)nugetFramework));
        }

        [Fact]
        public void AsFallbackFramework_WhenCalledMultipleTimes_CachesFallbackObjectReference()
        {
            var nugetFramework = CommonFrameworks.Net50;
            var dualCompatibilityFramework = new DualCompatibilityFramework(nugetFramework, secondaryFramework: NuGetFramework.AnyFramework);

            FallbackFramework fallbackFramework = dualCompatibilityFramework.AsFallbackFramework();

            var comparer = new NuGetFrameworkFullComparer();
            Assert.True(comparer.Equals(fallbackFramework, nugetFramework));

            fallbackFramework.Fallback.Should().HaveCount(1);
            fallbackFramework.Fallback.Single().Should().Be(NuGetFramework.AnyFramework);

            FallbackFramework fallbackFramework2 = dualCompatibilityFramework.AsFallbackFramework();
            fallbackFramework.Should().BeSameAs(fallbackFramework2);
        }
    }
}
