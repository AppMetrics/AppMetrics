// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Sampling.Interfaces;

namespace App.Metrics.Core.Options
{
    /// <summary>
    ///     Configuration of a Metric that will be measured using a specified <see cref="SamplingType" />
    /// </summary>
    public abstract class MetricValueWithSamplingOption : MetricValueOptions
    {
        /// <summary>
        ///     Gets or sets the <see cref="SamplingType" /> to use for the metric being measured.
        /// </summary>
        /// <remarks>
        ///     Sampling avoid unbound memory usage, allows metrics to be generated from a reservoir of values.
        /// </remarks>
        public SamplingType SamplingType { get; set; }

        /// <summary>
        ///     Gets or sets an <see cref="IReservoir" /> implementation to be used instead of the <see cref="SamplingType" />
        ///     specified
        /// </summary>
        public Func<IReservoir> WithReservoir { get; set; }
    }
}