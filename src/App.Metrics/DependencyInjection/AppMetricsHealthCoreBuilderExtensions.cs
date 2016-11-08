// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.DependencyInjection
{
    public static class AppMetricsHealthCoreBuilderExtensions
    {
        public static IMetricsHostBuilder AddHealthChecks(this IMetricsHostBuilder host)
        {
            host.AddHealthChecks(setupAction: null);
            return host;
        }

        public static IMetricsHostBuilder AddHealthChecks(
            this IMetricsHostBuilder host,
            Action<AppMetricsHealthCheckOptions> setupAction)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));

            host.AddMetricsHealthCheckCore(setupAction);

            return host;
        }
    }
}