// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics;
using App.Metrics.Internal;
using Microsoft.Extensions.PlatformAbstractions;
#if NET452
using System.Reflection;

#endif

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper restore CheckNamespace
    internal sealed class MetricsHostBuilder : IMetricsHostBuilder
    {
#if NET452
        [AppMetricsExcludeFromCodeCoverage] // DEVNOTE: Excluding for now, don't think the it's worth the effort in testing net452 at this time.
        internal MetricsHostBuilder(IServiceCollection services, AssemblyName assemblyName)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (assemblyName == null)
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }

            Services = services;
            Environment = new MetricsAppEnvironment(PlatformServices.Default.Application, assemblyName);
        }
#endif

#if !NET452
        internal MetricsHostBuilder(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            Services = services;
            Environment = new MetricsAppEnvironment(PlatformServices.Default.Application);
        }
#endif

        public IMetricsEnvironment Environment { get; }

        public IServiceCollection Services { get; }
    }
}