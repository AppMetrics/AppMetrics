// <copyright file="ScaledValueProvider{T}.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics
{
    public sealed class ScaledValueProvider<T> : IMetricValueProvider<T>
    {
        private readonly Func<T, T> _scalingFunction;

        public ScaledValueProvider(IMetricValueProvider<T> valueProvider, Func<T, T> transformation)
        {
            ValueProvider = valueProvider;
            _scalingFunction = transformation;
        }

        public T Value => _scalingFunction(ValueProvider.Value);

        public IMetricValueProvider<T> ValueProvider { get; }

        public T GetValue(bool resetMetric = false) { return _scalingFunction(ValueProvider.GetValue(resetMetric)); }
    }
}