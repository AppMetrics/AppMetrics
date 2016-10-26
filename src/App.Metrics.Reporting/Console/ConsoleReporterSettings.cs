using System;

namespace App.Metrics.Reporting.Console
{
    public class ConsoleReporterSettings : IConsoleReporterSettings
    {
        public TimeSpan Interval { get; set; }

        public bool Disabled { get; set; }
    }
}