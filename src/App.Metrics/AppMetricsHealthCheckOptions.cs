using System;
using App.Metrics.DependencyInjection;

namespace App.Metrics
{
    public class AppMetricsHealthCheckOptions
    {
        public AppMetricsHealthCheckOptions()
        {
            IsEnabled = true;
            HealthChecks = factory => { };
        }

        public Action<IHealthCheckFactory> HealthChecks { get; set; }


        public bool IsEnabled { get; set; }
    }
}