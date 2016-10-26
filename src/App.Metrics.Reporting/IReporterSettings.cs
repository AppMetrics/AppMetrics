using System;

namespace App.Metrics.Reporting
{
    public interface IReporterSettings
    {
        TimeSpan Interval { get; }

        bool Disabled { get; }
    }
}