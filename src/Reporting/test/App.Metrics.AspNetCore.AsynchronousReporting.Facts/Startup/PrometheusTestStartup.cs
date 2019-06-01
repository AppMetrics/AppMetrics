using System;

namespace App.Metrics.AspNetCore.AsynchronousReporting.Facts.Startup
{
    public class PrometheusTestStartup : DefaultTestStartup
    {
        public PrometheusTestStartup() : base(builder => builder.AsPrometheusPlainText()) { }
    }
}