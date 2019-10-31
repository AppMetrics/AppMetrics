// <copyright file="FunctionGauge.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Gauge
{
    /// <summary>
    ///     A Gauge metric using a function to provide the instantaneous value to record
    /// </summary>
    /// <seealso cref="IGaugeMetric" />
    public class FunctionGauge : IGaugeMetric
    {
        private readonly Func<double> _valueProvider;

        public FunctionGauge(Func<double> valueProvider) { _valueProvider = valueProvider; }

        /// <inheritdoc />
        public double Value
        {
            get
            {
                try
                {
                    return _valueProvider();
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
            throw new InvalidOperationException("Unable to reset a Function Gauge");
        }

        /// <inheritdoc />
        public void SetValue(double value)
        {
            throw new InvalidOperationException("Unable to set the value of a Function Gauge");
        }
    }
}