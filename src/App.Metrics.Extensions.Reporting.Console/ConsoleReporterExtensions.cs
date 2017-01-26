// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Abstractions.Filtering;
using App.Metrics.Reporting.Abstractions;

namespace App.Metrics.Extensions.Reporting.Console
{
    public static class ConsoleReporterExtensions
    {
        public static IReportFactory AddConsole(
            this IReportFactory factory,
            ConsoleReporterSettings settings,
            IFilterMetrics filter = null)
        {
            factory.AddProvider(new ConsoleReporterProvider(settings, filter));
            return factory;
        }

        public static IReportFactory AddConsole(
            this IReportFactory factory,
            IFilterMetrics filter = null)
        {
            var settings = new ConsoleReporterSettings();
            factory.AddConsole(settings, filter);
            return factory;
        }
    }
}