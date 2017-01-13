// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy

using App.Metrics.Data.Interfaces;

namespace App.Metrics.Data
{
    /// <summary>
    ///     Provides the value of a metric and information about units.
    ///     This is the class that metric consumers should use.
    /// </summary>
    /// <typeparam name="T">Type of the metric value</typeparam>
    public abstract class MetricValueSource<T>
    {
        protected MetricValueSource(string name, IMetricValueProvider<T> valueProvider, Unit unit, MetricTags tags)
        {
            Name = name;
            Unit = unit;
            ValueProvider = valueProvider;
            Tags = tags;
        }

        /// <summary>
        ///     Gets the Name of the metric.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        ///     Gets the Tags associated with the metric.
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public MetricTags Tags { get; private set; }

        /// <summary>
        ///     Gets the Unit representing what the metric is measuring.
        /// </summary>
        /// <value>
        ///     The unit.
        /// </value>
        public Unit Unit { get; private set; }

        /// <summary>
        ///     Gets the current value of the metric.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        public T Value => ValueProvider.Value;

        /// <summary>
        ///     Gets the instance capable of returning the current value for the metric.
        /// </summary>
        /// <value>
        ///     The value provider.
        /// </value>
        public IMetricValueProvider<T> ValueProvider { get; }
    }
}