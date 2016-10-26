using System;

namespace App.Metrics.Reporting.TextFile
{
    public class TextFileReporterSettings : ITextFileReporterSettings
    {
        public string FileReportingFolder { get; set; }

        public TimeSpan Interval { get; set; }

        public bool Disabled { get; set; }
    }
}