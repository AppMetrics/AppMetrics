// <copyright file="DerivedGauge.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Gauge
{
    public sealed class DerivedGauge : IGaugeMetric
    {
        private readonly IMetricValueProvider<double> _gauge;
        private readonly Func<double, double> _transformation;

        public DerivedGauge(IMetricValueProvider<double> gauge, Func<double, double> transformation)
        {
            _gauge = gauge;
            _transformation = transformation;
        }

        /// <inheritdoc />
        public double Value
        {
            get
            {
                try
                {
                    return _transformation(_gauge.Value);
                }
                catch (Exception)
                {
                    // TODO: https://github.com/AppMetrics/AppMetrics/issues/502
                    // return double.NaN;
                    return 0;
                }
            }
        }

        /// <inheritdoc />
        public double GetValue(bool resetMetric = false)
        {
            return Value;
        }

        /// <inheritdoc />
        public void Reset()
        {
            throw new InvalidOperationException("Unable to reset a Derived Gauge");
        }

        /// <inheritdoc />
        public void SetValue(double value)
        {
            throw new InvalidOperationException("Unable to set the value of a Derived Gauge");
        }
    }
}