// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


namespace App.Metrics.MetricData
{
    public static class ConstantValue
    {
        public static IMetricValueProvider<T> Provider<T>(T value)
        {
            return new ConstantValueProvider<T>(value);
        }

        private sealed class ConstantValueProvider<T> : IMetricValueProvider<T>
        {
            public ConstantValueProvider(T value)
            {
                Value = value;
            }

            public T Value { get; }

            public T GetValue(bool resetMetric = false)
            {
                return Value;
            }
        }
    }
}