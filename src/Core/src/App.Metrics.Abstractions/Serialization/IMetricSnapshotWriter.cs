// <copyright file="IMetricSnapshotWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace App.Metrics.Serialization
{
    public interface IMetricSnapshotWriter : IAsyncDisposable
    {
        /// <summary>
        /// Writes the specific metrics and tags
        /// </summary>
        /// <param name="context">The metric's context</param>
        /// <param name="name">The name of the metric</param>
        /// <param name="field">The label for the metric value</param>
        /// <param name="value">The value of the metrics</param>
        /// <param name="tags">The metric's tags</param>
        /// <param name="timestamp">The timestamp of the metrics snapshot</param>
        void Write(
            string context,
            string name,
            string field,
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