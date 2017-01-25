// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Data.Interfaces;

namespace App.Metrics.Meter.Interfaces
{
    /// <summary>
    ///     Provides access to a meter metric implementation e.g. <see cref="DefaultMeterMetric" />, allows custom meters to be
    ///     implemented
    /// </summary>
    /// <seealso cref="IMeter" />
    /// <seealso cref="IMetricValueProvider{T}" />
    public interface IMeterMetric : IMeter, IMetricValueProvider<MeterValue>, IDisposable
    {
    }
}