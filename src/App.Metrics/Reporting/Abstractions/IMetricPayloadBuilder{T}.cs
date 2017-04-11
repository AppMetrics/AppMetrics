// <copyright file="IMetricPayloadBuilder{T}.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Tagging;

namespace App.Metrics.Reporting.Abstractions
{
    public interface IMetricPayloadBuilder<out T>
    {
        void Clear();

        void Init();

        void Pack(string name, object value, MetricTags tags);

        void Pack(
            string name,
            IEnumerable<string> columns,
            IEnumerable<object> values,
            MetricTags tags);

        T Payload();

        string PayloadFormatted();
    }
}