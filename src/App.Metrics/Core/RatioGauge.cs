// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

//Written by Iulian Margarintescu and will retain the same license as the Java Version
//Original .NET Source by Iulian Margarintescu: https://github.com/etishor/Metrics.NET/tree/master/Src
//Ported to a .NET Standard Project by Allan Hardy as the owner Iulian Margarintescu is unreachable and the source and packages are no longer maintained


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
            : base(() => numerator() / denominator())
        {
        }
    }
}