using System;
using System.Collections.Generic;
using System.Diagnostics;
using App.Metrics.Core;
using App.Metrics.Reporters;
using App.Metrics.Utils;

namespace App.Metrics
{
    public class AppMetricsOptions
    {
        public IAppMetricsEvents Events = new AppMetricsEvents();

        private static readonly string DefaultGlobalContextName = $@"{CleanName(Environment.MachineName)}.{CleanName(Process.GetCurrentProcess().ProcessName)}";

        public AppMetricsOptions()
        {
            GlobalContextName = DefaultGlobalContextName;
            DisableMetrics = false;
            DefaultSamplingType = SamplingType.ExponentiallyDecaying;
        }

        public SamplingType DefaultSamplingType { get; set; }

        public bool DisableMetrics { get; set; }

        public string GlobalContextName { get; set; }

        public IClock SystemClock { get; set; } = Clock.Default;

        public IMetricsContext MetricsContext { get; set; } = new DefaultMetricsContext(DefaultGlobalContextName, Clock.Default);

        public IList<IMetricsReport> Reporters { get; } = new List<IMetricsReport>();

        private static string CleanName(string name)
        {
            return name.Replace('.', '_');
        }
    }
}