// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using App.Metrics.Reporting;
using App.Metrics.Reporting.Interfaces;

namespace App.Metrics.Extensions.Reporting.TextFile
{
    public static class TextFileReporterExtensions
    {
        public static IReportFactory AddTextFile(this IReportFactory factory,
            ITextFileReporterSettings settings, IMetricsFilter filter = null)
        {
            factory.AddProvider(new TextFileReporterProvider(settings, filter));
            return factory;
        }

        public static IReportFactory AddTextFile(this IReportFactory factory, IMetricsFilter filter = null)
        {
            var settings = new TextFileReporterSettings();
            factory.AddTextFile(settings, filter);
            return factory;
        }
    }
}