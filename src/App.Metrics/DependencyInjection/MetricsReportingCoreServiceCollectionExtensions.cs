// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Internal;
using App.Metrics.Reporting;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection.Extensions
// ReSharper restore CheckNamespace
{
    internal static class MetricsReportingCoreServiceCollectionExtensions
    {
        internal static IMetricsHost AddMetricsReportingCore(this IMetricsHost host)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            return AddMetricsReportingCore(host, setupAction: null);
        }

        internal static IMetricsHost AddMetricsReportingCore(
            this IMetricsHost host,
            Action<AppMetricsReportingOptions> setupAction)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));

            host.Services.TryAddSingleton<AppMetricsReportingMarkerService, AppMetricsReportingMarkerService>();

            host.Services.ConfigureDefaultReportingServices();
            host.Services.AddMetricsReportingCoreServices();

            if (setupAction != null)
            {
                host.Services.Configure(setupAction);
            }

            return host;
        }

        private static void AddMetricsReportingCoreServices(this IServiceCollection services)
        {
            services.TryAddSingleton(typeof(IReportFactory), provider =>
            {
                var options = provider.GetRequiredService<IOptions<AppMetricsReportingOptions>>();
                var factory = new ReportFactory();
                options.Value.Reporters(factory);
                return factory;
            });
        }

        private static void ConfigureDefaultReportingServices(this IServiceCollection services)
        {
            services.AddOptions();
        }
    }
}