using System;
using App.Metrics.Internal;

namespace App.Metrics.Reporting.TextFile
{
    public static class TextFileReporterExtensions
    {
        public static IReportFactory AddTextFile(this IReportFactory factory)
        {
            return factory.AddTextFile(
                new TextFileReporterSettings
                {
                  FileReportingFolder = @"C:\app-metrics\",
                  Disabled = false,
                  Interval = TimeSpan.FromSeconds(5)
                },
                new NoOpFilter());
        }

        public static IReportFactory AddTextFile(this IReportFactory factory,
            ITextFileReporterSettings settings)
        {
            factory.AddProvider(new TextFileReporterProvider(settings, new NoOpFilter()));
            return factory;
        }

        public static IReportFactory AddTextFile(this IReportFactory factory, 
            ITextFileReporterSettings settings,
            IMetricsFilter filter)
        {
            factory.AddProvider(new TextFileReporterProvider(settings, filter));
            return factory;
        }
    }
}