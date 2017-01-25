// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Data.Interfaces;
using App.Metrics.Histogram;
using App.Metrics.Histogram.Interfaces;

namespace App.Metrics.Abstractions.Metrics
{
    /// <summary>
    ///     Provides access to a histgram metric implementation e.g. <see cref="HistogramMetric" />, allows custom histograms
    ///     to be implemented
    /// </summary>
    /// <seealso cref="IHistogram" />
    /// <seealso cref="IMetricValueProvider{T}" />
    public interface IHistogramMetric : IHistogram, IMetricValueProvider<HistogramValue>, IDisposable
    {
    }
}