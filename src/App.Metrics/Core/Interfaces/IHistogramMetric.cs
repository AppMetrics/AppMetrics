// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Data;
using App.Metrics.Data.Interfaces;

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.Core.Interfaces
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