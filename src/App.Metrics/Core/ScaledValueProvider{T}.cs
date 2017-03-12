// <copyright file="ScaledValueProvider{T}.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Abstractions;

namespace App.Metrics.Core
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

        // ReSharper disable MemberCanBePrivate.Global
        public IMetricValueProvider<T> ValueProvider { get; }
        // ReSharper restore MemberCanBePrivate.Global

        public T GetValue(bool resetMetric = false) { return _scalingFunction(ValueProvider.GetValue(resetMetric)); }
    }
}