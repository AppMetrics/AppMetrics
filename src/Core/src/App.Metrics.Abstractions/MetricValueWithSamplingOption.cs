// <copyright file="MetricValueWithSamplingOption.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.ReservoirSampling;

namespace App.Metrics
{
    /// <summary>
    ///     Configuration of a Metric that will be measured using a reservoir sampling type
    /// </summary>
    public abstract class MetricValueWithSamplingOption : MetricValueOptionsBase
    {
        /// <summary>
        ///     Gets or sets an <see cref="IReservoir" /> implementation for sampling.
        /// </summary>
        /// <value>
        ///     The reservoir instance to use for sampling.
        /// </value>
        /// <remarks>
        ///     Reservoir sampling avoids unbound memory usage, allows metrics to be generated from a reservoir of values.
        /// </remarks>
        public Func<IReservoir> Reservoir { get; set; }
    }
}