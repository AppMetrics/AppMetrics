using System;
using System.Diagnostics;
using App.Metrics.Json;
using App.Metrics.Registries;
using App.Metrics.Utils;

namespace App.Metrics
{
    public sealed class AppMetricsOptions
    {
        public IAppMetricsEvents Events = new AppMetricsEvents();

        private static readonly string DefaultGlobalContextName =
            $@"{CleanName(Environment.MachineName)}.{CleanName(Process.GetCurrentProcess().ProcessName)}";

        public AppMetricsOptions()
        {
            GlobalContextName = DefaultGlobalContextName;
            DisableMetrics = false;
            DisableHealthChecks = false;
            DefaultSamplingType = SamplingType.ExponentiallyDecaying;
            Reporters = reports => { };
            HealthCheckRegistry = checks => { };
        }

        public SamplingType DefaultSamplingType { get; set; }

        public bool DisableHealthChecks { get; set; }

        public bool DisableMetrics { get; set; }

        public bool EnableInternalMetrics { get; set; }

        public string GlobalContextName { get; set; }

        public Action<IHealthCheckRegistry> HealthCheckRegistry { get; set; }

        public JsonSchemeVersion JsonSchemeVersion { get; set; } = JsonSchemeVersion.AlwaysLatest;

        public Action<IMetricReporterRegistry> Reporters { get; set; }

        public IClock SystemClock { get; set; } = Clock.Default;

        private static string CleanName(string name)
        {
            return name.Replace('.', '_');
        }
    }
}