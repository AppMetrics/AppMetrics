// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core.Options;

namespace App.Metrics.Histogram.Abstractions
{
    public interface IProvideHistogramMetrics
    {
        /// <summary>
        ///     Instantiates an instance of a <see cref="IHistogram" />
        /// </summary>
        /// <param name="options">The details of the histogram that is being measured</param>
        /// <returns>A new instance of an <see cref="IHistogram" /> or the existing registered instance of the histogram</returns>
        IHistogram Instance(HistogramOptions options);

        /// <summary>
        ///     Instantiates an instance of a <see cref="IHistogram" />
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IHistogram" /> to instantiate</typeparam>
        /// <param name="options">The details of the <see cref="IHistogram" /> that is being measured</param>
        /// <param name="builder">The function used to build the histogram metric.</param>
        /// <returns>A new instance of an <see cref="IHistogram" /> or the existing registered instance of the histogram</returns>
        IHistogram Instance<T>(HistogramOptions options, Func<T> builder)
            where T : IHistogramMetric;
    }
}