// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Core.Options;

namespace App.Metrics.Counter.Abstractions
{
    public interface IProvideCounterMetrics
    {
        ICounter Instance(CounterOptions options);

        ICounter Instance<T>(CounterOptions options, Func<T> builder)
            where T : ICounterMetric;
    }
}