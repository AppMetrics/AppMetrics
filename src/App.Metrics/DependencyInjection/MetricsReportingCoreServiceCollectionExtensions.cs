// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Internal;
using App.Metrics.Reporting;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection.Extensions
// ReSharper restore CheckNamespace
{
    internal static class MetricsReportingCoreServiceCollectionExtensions
    {
        internal static IMetricsHostBuilder AddMetricsReportingCore(this IMetricsHostBuilder host)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            return AddMetricsReportingCore(host, setupAction: null);
        }

        internal static IMetricsHostBuilder AddMetricsReportingCore(
            this IMetricsHostBuilder host,
            Action<AppMetricsReportingOptions, IReportFactory> setupAction)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));

            host.Services.TryAddSingleton<AppMetricsReportingMarkerService, AppMetricsReportingMarkerService>();

            host.Services.ConfigureDefaultReportingServices();
            host.Services.AddMetricsReportingCoreServices(setupAction);

            return host;
        }

        private static void AddMetricsReportingCoreServices(this IServiceCollection services,
            Action<AppMetricsReportingOptions, IReportFactory> setupAction)
        {
            var factory = new ReportFactory();
            var options = new AppMetricsReportingOptions();

            setupAction(options, factory);

            if (!options.IsEnabled)
            {
                services.TryAddSingleton<IReportFactory>(new NullReportFactory());
            }
            else
            {
                services.TryAddSingleton<IReportFactory>(factory);
            }

            services.TryAddSingleton(options);
        }

        private static void ConfigureDefaultReportingServices(this IServiceCollection services)
        {
            services.AddOptions();
        }
    }
}