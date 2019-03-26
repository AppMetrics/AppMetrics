// <copyright file="DefaultGaugeBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

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