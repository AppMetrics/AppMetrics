// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Reporting;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection.Extensions
// ReSharper restore CheckNamespace
{
    public static class AppMetricsReportingCoreBuilderExtensions
    {
        public static IMetricsHost AddReporting(this IMetricsHost host)
        {
            host.AddReporting(setupAction: null);
            return host;
        }

        public static IMetricsHost AddReporting(
            this IMetricsHost host,
            Action<AppMetricsReportingOptions> setupAction)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));

            host.AddMetricsReportingCore(setupAction);

            return host;
        }
    }
}