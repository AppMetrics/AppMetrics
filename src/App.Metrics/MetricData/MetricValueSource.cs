// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System.Collections.Generic;
using App.Metrics.Utils;

namespace App.Metrics.MetricData
{
    /// <summary>
    ///     Provides the value of a metric and information about units.
    ///     This is the class that metric consumers should use.
    /// </summary>
    /// <typeparam name="T">Type of the metric value</typeparam>
    public abstract class MetricValueSource<T> : IHideObjectMembers
    {
        protected MetricValueSource(string name, IMetricValueProvider<T> valueProvider, Unit unit, MetricTags tags)
        {
            Name = name;
            Unit = unit;
            ValueProvider = valueProvider;
            Tags = tags;
        }

        /// <summary>
        ///     Name of the metric.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Tags associated with the metric.
        /// </summary>
        public MetricTags Tags { get; private set; }

        /// <summary>
        ///     Unit representing what the metric is measuring.
        /// </summary>
        public Unit Unit { get; private set; }

        /// <summary>
        ///     The current value of the metric.
        /// </summary>
        public T Value => ValueProvider.Value;

        /// <summary>
        ///     Instance capable of returning the current value for the metric.
        /// </summary>
        public IMetricValueProvider<T> ValueProvider { get; }
    }
}