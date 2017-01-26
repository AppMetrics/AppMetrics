// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Gauge.Abstractions;

namespace App.Metrics.Gauge
{
    public class DefaultGaugeBuilder : IBuildGaugeMetrics
    {
        /// <inheritdoc />
        public IGaugeMetric Build(Func<double> valueProvider) { return new FunctionGauge(valueProvider); }
    }
}