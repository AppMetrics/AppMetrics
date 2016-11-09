// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using App.Metrics.Internal;
using App.Metrics.Utils;

namespace App.Metrics
{
    public class AppMetricsOptions
    {
        private const string DefaultContext = "Application";

        public AppMetricsOptions()
        {
            DefaultContextLabel = DefaultContext;
            DisableMetrics = false;
            DefaultSamplingType = SamplingType.ExponentiallyDecaying;
            MetricsFilter = new DefaultMetricsFilter();
            Clock = Utils.Clock.Default;
        }

        public IClock Clock { get; set; } //TODO: AH - remove clock elsewhere

        public string DefaultContextLabel { get; set; }

        public SamplingType DefaultSamplingType { get; set; }

        public bool DisableMetrics { get; set; }

        public IMetricsFilter MetricsFilter { get; set; } //TODO: AH - remove filter elsewhere
    }
}