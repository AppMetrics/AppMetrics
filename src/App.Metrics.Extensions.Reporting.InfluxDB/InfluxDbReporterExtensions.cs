// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using App.Metrics.Extensions.Reporting.InfluxDB;

// ReSharper disable CheckNamespace
namespace App.Metrics.Reporting.Interfaces
// ReSharper restore CheckNamespace
{
    public static class InfluxDbReporterExtensions
    {
        public static IReportFactory AddInfluxDb(this IReportFactory factory,
            InfluxDBReporterSettings settings, IMetricsFilter filter = null)
        {
            factory.AddProvider(new InfluxDbReporterProvider(settings, filter));
            return factory;
        }

        public static IReportFactory AddInfluxDb(this IReportFactory factory, 
            IMetricsFilter filter = null)
        {
            var settings = new InfluxDBReporterSettings();
            factory.AddInfluxDb(settings, filter);
            return factory;
        }
    }
}