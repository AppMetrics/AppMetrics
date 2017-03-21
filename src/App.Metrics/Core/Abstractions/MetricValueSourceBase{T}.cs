// <copyright file="MetricValueSourceBase{T}.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Internal;
using App.Metrics.Tagging;

// ReSharper disable MemberCanBePrivate.Global
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

            if (name.Contains(Constants.Formatting.MetricNameDimensionSeparator))
            {
                IsMultidimensional = true;
                MultidimensionalName =
                    name.Split(new[] { Constants.Formatting.MetricNameDimensionSeparator }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is a multidimensional metric.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is a multidimensional metric; otherwise, <c>false</c>.
        /// </value>
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public bool IsMultidimensional { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        /// <summary>
        ///     Gets the name of the multidimensional metric. If tags were set at runtime, this will be the name of the metric
        ///     without the concatenated metric tags so that these metrics can be reported as one.
        /// </summary>
        /// <value>
        ///     The name of the multidimensional metric.
        /// </value>
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public string MultidimensionalName { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

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

    // ReSharper restore MemberCanBePrivate.Global
}