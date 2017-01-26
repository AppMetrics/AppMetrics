// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.MetricTypes;
using App.Metrics.Counter.Abstractions;

namespace App.Metrics.Counter
{
    public class DefaultCounterBuilder : IBuildCounterMetrics
    {
        /// <inheritdoc />
        public ICounterMetric Build() { return new DefaultCounterMetric(); }

        /// <inheritdoc />
        public ICounterMetric Build<T>(Func<T> builder)
            where T : ICounterMetric { return builder(); }
    }
}