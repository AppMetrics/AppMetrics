// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.MetricTypes;

namespace App.Metrics.Gauge
{
    /// <summary>
    ///     A Gauge metric using two functions to calculate a percentage
    /// </summary>
    /// <seealso cref="IGaugeMetric" />
    public class PercentageGauge : FunctionGauge
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PercentageGauge" /> class.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        public PercentageGauge(Func<double> numerator, Func<double> denominator)
            : base(() =>
            {
                var ratio = numerator() / denominator();

                if (Math.Abs(ratio) < 0.0001)
                {
                    return 0;
                }

                if (ratio <= 1)
                {
                    return ratio * 100.0;
                }

                return 100.0;
            }) { }
    }
}