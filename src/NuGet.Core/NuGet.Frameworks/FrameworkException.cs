// Copyright (c) 2022-Present Chocolatey Software, Inc.
// Copyright (c) 2015-2022 .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;

//////////////////////////////////////////////////////////
// Start - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
namespace Chocolatey.NuGet.Frameworks
//////////////////////////////////////////////////////////
// End - Chocolatey Specific Modification
//////////////////////////////////////////////////////////
{
    [Serializable]
    public class FrameworkException : Exception
    {
        public FrameworkException(string message)
            : base(message)
        {
        }

        protected FrameworkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
