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