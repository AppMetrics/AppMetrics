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
            DefaultGroupName = "_default";
            DisableMetrics = false;
            DefaultSamplingType = SamplingType.ExponentiallyDecaying;
            MetricsFilter = new DefaultMetricsFilter();
            Clock = Utils.Clock.Default;
        }

        public SamplingType DefaultSamplingType { get; set; }

        public bool DisableMetrics { get; set; }

        public string DefaultGroupName { get; set; }

        public string GlobalContextName { get; set; }

        public JsonSchemeVersion JsonSchemeVersion { get; set; } = JsonSchemeVersion.AlwaysLatest;

        public IMetricsFilter MetricsFilter { get; set; }

        public IClock Clock { get; set; }

        private static string CleanName(string name)
        {
            return name.Replace('.', '_');
        }
    }
}