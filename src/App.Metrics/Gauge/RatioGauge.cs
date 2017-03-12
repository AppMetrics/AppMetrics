// <copyright file="RatioGauge.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Abstractions.MetricTypes;

namespace App.Metrics.Gauge
{
    /// <summary>
    ///     A Gauge metric using two functions to calculate a ratio to record
    /// </summary>
    /// <seealso cref="IGaugeMetric" />
    public class RatioGauge : FunctionGauge
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RatioGauge" /> class.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        // ReSharper disable MemberCanBeProtected.Global
        public RatioGauge(Func<double> numerator, Func<double> denominator)
            // ReSharper restore MemberCanBeProtected.Global
            : base(() => numerator() / denominator()) { }
    }
}