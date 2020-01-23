// <copyright file="PercentageGauge.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

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
        // ReSharper disable MemberCanBeProtected.Global
        public PercentageGauge(Func<double> numerator, Func<double> denominator)
            // ReSharper restore MemberCanBeProtected.Global
            : base(() =>
            {
                var ratio = numerator() / denominator();

                if (double.IsNaN(ratio) || double.IsInfinity(ratio))
                {
                    // TODO: https://github.com/AppMetrics/AppMetrics/issues/502
                    // return double.NaN;
                    return 0;
                }

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