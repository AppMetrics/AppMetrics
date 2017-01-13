// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy

using App.Metrics.Data.Interfaces;

namespace App.Metrics.Core.Interfaces
{
    /// <summary>
    ///     Provides access to a gauge metric implementation e.g. <see cref="FunctionGauge" />, <see cref="HitRatioGauge" />,
    ///     allows custom gauges to be implemented
    /// </summary>
    /// <seealso cref="IGaugeMetric" />
    public interface IGaugeMetric : IMetricValueProvider<double>
    {
    }
}