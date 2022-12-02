// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Shared;

//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
namespace Chocolatey.NuGet.Frameworks
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
{
    public class FrameworkRangeComparer : IEqualityComparer<FrameworkRange>
    {
        public FrameworkRangeComparer()
        {
        }

        public bool Equals(FrameworkRange x, FrameworkRange y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null)
                || ReferenceEquals(y, null))
            {
                return false;
            }

            return StringComparer.OrdinalIgnoreCase.Equals(x.FrameworkIdentifier, y.FrameworkIdentifier) &&
                   NuGetFramework.Comparer.Equals(x.Min, y.Min) && NuGetFramework.Comparer.Equals(x.Max, y.Max)
                   && x.IncludeMin == y.IncludeMin && x.IncludeMax == y.IncludeMax;
        }

        public int GetHashCode(FrameworkRange obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return 0;
            }

            var combiner = new HashCodeCombiner();

            combiner.AddStringIgnoreCase(obj.FrameworkIdentifier);
            combiner.AddObject(obj.Min);
            combiner.AddObject(obj.Max);
            combiner.AddObject(obj.IncludeMin);
            combiner.AddObject(obj.IncludeMax);

            return combiner.CombinedHash;
        }
    }
}
