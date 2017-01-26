// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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

        public IMetricValueProvider<T> ValueProvider { get; }

        public T GetValue(bool resetMetric = false) { return _scalingFunction(ValueProvider.GetValue(resetMetric)); }
    }
}