// <copyright file="CustomGauge.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Gauge;

namespace App.Metrics.Facts.TestHelpers
{
    public class CustomGauge : IGaugeMetric
    {
        /// <inheritdoc />
        public double Value => GetValue();

        /// <inheritdoc />
        public double GetValue(bool resetMetric = false)
        {
            return 1.0;
        }

        /// <inheritdoc />
        public void Reset()
        {
        }

        /// <inheritdoc />
        public void SetValue(double value)
        {
        }
    }
}