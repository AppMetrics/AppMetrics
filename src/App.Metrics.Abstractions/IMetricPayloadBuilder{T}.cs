// <copyright file="IMetricPayloadBuilder{T}.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics
{
    public interface IMetricPayloadBuilder<out T>
    {
        MetricValueDataKeys DataKeys { get; }

        void Clear();

        void Init();

        void Pack(string context, string name, object value, MetricTags tags);

        void Pack(
            string context,
            string name,
            IEnumerable<string> columns,
            IEnumerable<object> values,
            MetricTags tags);

        T Payload();

        string PayloadFormatted();
    }
}