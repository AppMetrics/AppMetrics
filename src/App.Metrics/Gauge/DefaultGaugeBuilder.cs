// <copyright file="DefaultGaugeBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core.Abstractions;
using App.Metrics.Gauge.Abstractions;

namespace App.Metrics.Gauge
{
    public class DefaultGaugeBuilder : IBuildGaugeMetrics
    {
        /// <inheritdoc />
        public IGaugeMetric Build(Func<double> valueProvider)
        {
            return new FunctionGauge(valueProvider);
        }

        public IGaugeMetric Build(Func<IMetricValueProvider<double>> valueProvider) { return new FunctionGauge(() => valueProvider().Value); }

        public IGaugeMetric Build() { return new ValueGauge(); }

        public IGaugeMetric Build<T>(Func<T> builder)
            where T : IGaugeMetric
        {
            return builder();
        }
    }
}