// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using FluentAssertions;
//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
using Chocolatey.NuGet.Frameworks;
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
using Xunit;

namespace NuGet.ProjectModel.Test
{
    public class LockFileTargetTests
    {
        [Theory]
        [InlineData("net461", "net461", true)]
        [InlineData("net461", "net462", false)]
        public void Equals_WithTargetFramework(string left, string right, bool expected)
        {
            var leftSide = new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse(left)
            };

            var rightSide = new LockFileTarget()
            {
                TargetFramework = NuGetFramework.Parse(right)
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }

        [Theory]
        [InlineData("win7", "win7", true)]
        [InlineData("win10", "win7", false)]
        public void Equals_WithRuntimeIdentifier(string left, string right, bool expected)
        {
            var leftSide = new LockFileTarget()
            {
                RuntimeIdentifier = left
            };

            var rightSide = new LockFileTarget()
            {
                RuntimeIdentifier = right
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }


        [Theory]
        [InlineData("project", "project", true)]
        [InlineData("project", "package", false)]
        [InlineData("project;project2", "project2;project", true)]
        [InlineData("project;project2", "project;project2;project3", false)]
        public void Equals_WithLockFileTargetLibraries(string left, string right, bool expected)
        {
            var leftSide = new LockFileTarget()
            {
                Libraries = left.Split(';').Select(e => new LockFileTargetLibrary() { Name = e }).ToList()
            };

            var rightSide = new LockFileTarget()
            {
                Libraries = right.Split(';').Select(e => new LockFileTargetLibrary() { Name = e }).ToList()
            };

            // Act & Assert
            if (expected)
            {
                leftSide.Should().Be(rightSide);
            }
            else
            {
                leftSide.Should().NotBe(rightSide);
            }
        }
    }
}
