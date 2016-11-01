using System;

namespace App.Metrics.Reporting
{
    public class ConsoleReporterSettings : IConsoleReporterSettings
    {
        public bool Disabled { get; set; } = false;

        public IMetricsFilter Filter { get; set; }

        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(5);

        public string OutputTemplate { get; set; } = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";
    }
}