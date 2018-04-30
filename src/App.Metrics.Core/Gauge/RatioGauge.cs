// <copyright file="RatioGauge.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

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