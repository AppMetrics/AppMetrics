// <copyright file="DefaultCounterBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

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