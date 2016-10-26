using System;

namespace App.Metrics.Reporting
{
    public class AppMetricsReportingOptions
    {
        public AppMetricsReportingOptions()
        {
            IsEnabled = true;
            Reporters = factory => { };
        }

        public bool IsEnabled { get; set; }

        public Action<IReportFactory> Reporters { get; set; }
    }
}