using System;
using App.Metrics.Internal;

namespace App.Metrics.Reporting.Console
{
    public static class ConsoleReporterExtensions
    {
        public static IReportFactory AddConsole(this IReportFactory factory)
        {
            return factory.AddConsole(
                new ConsoleReporterSettings
                {
                  Disabled  = false,
                  Interval = TimeSpan.FromSeconds(5)
                },
                new NoOpFilter());
        }

        public static IReportFactory AddConsole(this IReportFactory factory,
            IConsoleReporterSettings settings)
        {
            factory.AddProvider(new ConsoleReporterProvider(settings, new NoOpFilter()));
            return factory;
        }

        public static IReportFactory AddConsole(this IReportFactory factory,
            IConsoleReporterSettings settings,
            IMetricsFilter filter)
        {
            factory.AddProvider(new ConsoleReporterProvider(settings, filter));
            return factory;
        }
    }
}