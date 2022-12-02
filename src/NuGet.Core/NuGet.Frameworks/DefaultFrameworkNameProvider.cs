// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
namespace Chocolatey.NuGet.Frameworks
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
{
    public sealed class DefaultFrameworkNameProvider : FrameworkNameProvider
    {
        public DefaultFrameworkNameProvider()
            : base(new IFrameworkMappings[] { DefaultFrameworkMappings.Instance },
                new IPortableFrameworkMappings[] { DefaultPortableFrameworkMappings.Instance })
        {
        }

        private static readonly Lazy<IFrameworkNameProvider> InstanceLazy = new Lazy<IFrameworkNameProvider>(() => new DefaultFrameworkNameProvider());

        public static IFrameworkNameProvider Instance
        {
            get { return InstanceLazy.Value; }
        }
    }
}
