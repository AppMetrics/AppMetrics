// <copyright file="IHistogramMetric.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Abstractions;
using App.Metrics.Histogram;
using App.Metrics.Histogram.Abstractions;

namespace App.Metrics.Abstractions.MetricTypes
{
    /// <summary>
    ///     Provides access to a histgram metric implementation e.g. <see cref="DefaultHistogramMetric" />, allows custom
    ///     histograms to be implemented
    /// </summary>
    /// <seealso cref="IHistogram" />
    /// <seealso cref="IMetricValueProvider{T}" />
    public interface IHistogramMetric : IHistogram, IMetricValueProvider<HistogramValue>, IDisposable
    {
    }
}