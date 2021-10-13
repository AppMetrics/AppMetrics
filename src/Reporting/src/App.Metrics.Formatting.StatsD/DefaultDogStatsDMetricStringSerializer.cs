// <copyright file="DefaultStatsDMetricStringSerializer.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using App.Metrics.Formatting.StatsD.Internal;

namespace App.Metrics.Formatting.StatsD
{
    public class DefaultDogStatsDMetricStringSerializer : IStatsDMetricStringSerializer
    {
        /// <inheritdoc />
        public string Serialize(StatsDPoint point, MetricsStatsDOptions options)
        {
            var builder = new StringBuilder();
            builder.Append(point.Name);
            builder.Append(":");
            builder.Append(StatsDSyntax.FormatValue(point.Value, point.MetricType));
            builder.Append("|");
            builder.Append(point.MetricType);

            if (point.SampleRate.HasValue && point.SampleRate < 1.0)
            {
                builder.Append("|@");
                builder.Append(point.SampleRate.Value.ToString("0.###############", NumberFormatInfo.InvariantInfo));
            }

            if (point.Tags != null && point.Tags.Count > 0)
            {
                builder.Append('|');
                builder.Append(options.TagMarker);
                builder.Append(string.Join(",", point.Tags.Select(tag => $"{tag.Key}:{tag.Value}")));
            }
            
            return builder.ToString();
        }
    }
}
