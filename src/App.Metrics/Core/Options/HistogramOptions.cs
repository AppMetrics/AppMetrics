// <copyright file="HistogramOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Abstractions.MetricTypes;

namespace App.Metrics.Core.Options
{
    /// <summary>
    ///     Configuration of an <see cref="IHistogramMetric" /> that will be measured
    /// </summary>
    /// <seealso cref="MetricValueWithSamplingOption" />
    public class HistogramOptions : MetricValueWithSamplingOption
    {
    }
}