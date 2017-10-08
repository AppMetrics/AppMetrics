// <copyright file="IMetricSnapshotWriter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace App.Metrics.Serialization
{
    public interface IMetricSnapshotWriter : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="GeneratedMetricNameMapping"/> allowing customization of the metric names used for auto-generated metrics e.g. historgram percentailes
        /// </summary>
        /// <value>
        /// The generated metric name mappings
        /// </value>
        GeneratedMetricNameMapping MetricNameMapping { get; }

        /// <summary>
        /// Writes the specific metrics and tags
        /// </summary>
        /// <param name="context">The metric's context</param>
        /// <param name="name">The name of the metric</param>
        /// <param name="value">The value of the metrics</param>
        /// <param name="tags">The metric's tags</param>
        /// <param name="timestamp">The timestamp of the metrics snapshot</param>
        void Write(
            string context,
            string name,
            object value,
            MetricTags tags,
            DateTime timestamp);

        /// <summary>
        /// Writes the specific metrics and tags
        /// </summary>
        /// <param name="context">The metric's context</param>
        /// <param name="name">The name of the metric</param>
        /// <param name="columns">The metric names</param>
        /// <param name="values">The corresponding metrics values</param>
        /// <param name="tags">The metric's tags</param>
        /// <param name="timestamp">The timestamp of the metrics snapshot</param>
        void Write(
            string context,
            string name,
            IEnumerable<string> columns,
            IEnumerable<object> values,
            MetricTags tags,
            DateTime timestamp);
    }
}