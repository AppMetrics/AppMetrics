// <copyright file="DerivedGauge.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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
                    return double.NaN;
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