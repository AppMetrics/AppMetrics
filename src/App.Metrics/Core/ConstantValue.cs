// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Internal;

namespace App.Metrics.Core
{
    [AppMetricsExcludeFromCodeCoverage]
    public static class ConstantValue
    {
        public static IMetricValueProvider<T> Provider<T>(T value) { return new ConstantValueProvider<T>(value); }

        private sealed class ConstantValueProvider<T> : IMetricValueProvider<T>
        {
            public ConstantValueProvider(T value) { Value = value; }

            public T Value { get; }

            public T GetValue(bool resetMetric = false) { return Value; }
        }
    }
}