// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;

namespace App.Metrics.Configuration
{
    public sealed class AppMetricsOptions
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

        public Dictionary<string, string> GlobalTags { get; set; } = new Dictionary<string, string>();

    }
}