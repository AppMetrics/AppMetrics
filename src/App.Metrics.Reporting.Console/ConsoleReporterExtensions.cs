namespace App.Metrics.Reporting
{
    public static class ConsoleReporterExtensions
    {
        public static IReportFactory AddConsole(this IReportFactory factory,
            IConsoleReporterSettings settings)
        {
            factory.AddProvider(new ConsoleReporterProvider(settings));
            return factory;
        }

        public static IReportFactory AddConsole(this IReportFactory factory)
        {
            var settings = new ConsoleReporterSettings();
            factory.AddConsole(settings);
            return factory;
        }
    }
}