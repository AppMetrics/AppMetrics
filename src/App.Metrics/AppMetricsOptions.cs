// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Diagnostics;
using App.Metrics.Internal;
using App.Metrics.Json;
using App.Metrics.Registries;
using App.Metrics.Utils;

namespace App.Metrics
{
    public class AppMetricsOptions
    {
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
            MetricsFilter = new DefaultMetricsFilter();
        }

        public SamplingType DefaultSamplingType { get; set; }

        public bool DisableHealthChecks { get; set; }

        public bool DisableMetrics { get; set; }

        public string GlobalContextName { get; set; }

        public Action<IHealthCheckRegistry> HealthCheckRegistry { get; set; }

        public JsonSchemeVersion JsonSchemeVersion { get; set; } = JsonSchemeVersion.AlwaysLatest;

        public IMetricsFilter MetricsFilter { get; set; }

        public Action<IMetricReporterRegistry> Reporters { get; set; }

        public IClock SystemClock { get; set; } = Clock.Default;

        private static string CleanName(string name)
        {
            return name.Replace('.', '_');
        }
    }
}