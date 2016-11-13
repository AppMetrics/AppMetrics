// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Configuration
{
    public class AppMetricsOptions
    {
        private const string DefaultContext = "Application";

        public AppMetricsOptions()
        {
            DefaultContextLabel = DefaultContext;
            MetricsEnabled = true;
            ReportingEnabled = true;
            DefaultSamplingType = SamplingType.ExponentiallyDecaying;
        }

        public string DefaultContextLabel { get; set; }

        public SamplingType DefaultSamplingType { get; set; }

        public bool MetricsEnabled { get; set; }

        public bool ReportingEnabled { get; set; }
    }
}