// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio;
using Task = System.Threading.Tasks.Task;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public abstract class MockedVSCollectionTests : IAsyncServiceProvider, IDisposable
    {
        private readonly Dictionary<Type, Task<object>> _services = new Dictionary<Type, Task<object>>();
        protected readonly Dictionary<string, bool> _experimentationFlags;

        private readonly CultureInfo _originalCulture;
        private readonly CultureInfo _originalUiCulture;

        public virtual void Dispose()
        {
            CultureInfo.CurrentCulture = _originalCulture;
            CultureInfo.CurrentUICulture = _originalUiCulture;
        }

        public MockedVSCollectionTests(GlobalServiceProvider globalServiceProvider)
        {
            _originalCulture = CultureInfo.CurrentCulture;
            _originalUiCulture = CultureInfo.CurrentUICulture;
            CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("en");
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

            globalServiceProvider.Reset();
            _experimentationFlags = new Dictionary<string, bool>();

            ServiceLocator.InitializePackageServiceProvider(this);
        }

        protected void AddService<T>(Task<object> obj)
        {
            _services.Add(typeof(T), obj);
        }

        public Task<object> GetServiceAsync(Type serviceType)
        {
            if (_services.TryGetValue(serviceType, out Task<object> task))
            {
                return task;
            }

            return Task.FromResult<object>(null);
        }
    }
}
