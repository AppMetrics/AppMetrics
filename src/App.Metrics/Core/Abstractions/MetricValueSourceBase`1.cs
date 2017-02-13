// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Tagging;

namespace App.Metrics.Core.Abstractions
{
    /// <summary>
    ///     Provides the value of a metric and information about units.
    ///     This is the class that metric consumers should use.
    /// </summary>
    /// <typeparam name="T">Type of the metric value</typeparam>
    public abstract class MetricValueSourceBase<T>
    {
        protected MetricValueSourceBase(string name, IMetricValueProvider<T> valueProvider, Unit unit, MetricTags tags)
        {
            Name = name;
            Unit = unit;
            ValueProvider = valueProvider;
            Tags = tags;
        }

        protected MetricValueSourceBase(string name, string group, IMetricValueProvider<T> valueProvider, Unit unit, MetricTags tags)
        {
            Name = name;
            Group = group;
            Unit = unit;
            ValueProvider = valueProvider;
            Tags = tags;
        }

        /// <summary>
        ///     Gets the Group of the metric within a context.
        /// </summary>
        /// <value>
        ///     The group.
        /// </value>
        public string Group { get; }

        /// <summary>
        ///     Gets the Name of the metric.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        ///     Gets the Tags associated with the metric.
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public MetricTags Tags { get; }

        /// <summary>
        ///     Gets the Unit representing what the metric is measuring.
        /// </summary>
        /// <value>
        ///     The unit.
        /// </value>
        public Unit Unit { get; }

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