// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
namespace Chocolatey.NuGet.Frameworks
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
{
    public sealed class DefaultCompatibilityProvider : CompatibilityProvider
    {
        public DefaultCompatibilityProvider()
            : base(DefaultFrameworkNameProvider.Instance)
        {
        }

        private static IFrameworkCompatibilityProvider _instance;

        public static IFrameworkCompatibilityProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DefaultCompatibilityProvider();
                }

                return _instance;
            }
        }
    }
}
