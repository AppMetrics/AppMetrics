// <copyright file="ConstantValue.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics
{
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