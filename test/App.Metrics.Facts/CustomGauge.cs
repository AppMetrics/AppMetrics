// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Abstractions.MetricTypes;

namespace App.Metrics.Facts
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