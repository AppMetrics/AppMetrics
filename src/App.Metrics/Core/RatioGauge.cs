// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy

using System;
using App.Metrics.Core.Interfaces;

namespace App.Metrics.Core
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
        public RatioGauge(Func<double> numerator, Func<double> denominator)
            : base(() => numerator() / denominator()) { }
    }
}