// <copyright file="DefaultCounterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

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