// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Interfaces;
using App.Metrics.Internal;

namespace App.Metrics.Core.Options
{
    /// <summary>
    ///     Configuration of an <see cref="IHistogramMetric" /> that will be measured
    /// </summary>
    /// <seealso cref="MetricValueOptions" />
    public class HistogramOptions : MetricValueWithSamplingOption
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HistogramOptions" /> class.
        /// </summary>
        public HistogramOptions()
        {
            SamplingType = SamplingType.Default;
            SampleSize = Constants.ReservoirSampling.DefaultSampleSize;
            ExponentialDecayFactor = Constants.ReservoirSampling.DefaultExponentialDecayFactor;
        }
    }
}