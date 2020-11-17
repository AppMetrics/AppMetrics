// <copyright file="StatsDPointSampler.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Metrics.Formatting.StatsD.Internal
{
    internal class StatsDPointSampler
    {
        private static readonly HashSet<string> ExcludeTags = new HashSet<string>
        {
            AppMetricsConstants.Pack.MetricTagsTypeKey,
            StatsDFormatterConstants.SampleRateTagName,
            StatsDFormatterConstants.ItemTagName
        };

        private readonly MetricsStatsDOptions _options;
        private readonly ConcurrentDictionary<string, StatsDSampler> _samplers = new ConcurrentDictionary<string, StatsDSampler>();

        public StatsDPointSampler(MetricsStatsDOptions options)
        {
            _options = options;
        }

        public ConcurrentQueue<StatsDPoint> Points { get; } = new ConcurrentQueue<StatsDPoint>();

        public void Add(
            string context, 
            string name, 
            string field, 
            object value, 
            MetricTags metricTags, 
            IStatsDMetricStringSerializer serializer, 
            DateTime? timestamp)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Name could not be null or whitespace");

            if (string.IsNullOrWhiteSpace(field))
                throw new ArgumentNullException(nameof(field), "Field could not be null or whitespace");

            var tagsDictionary = metricTags.ToDictionary();

            var tags = tagsDictionary
                .Where(tag => !ExcludeTags.Contains(tag.Key))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            tagsDictionary.TryGetValue(AppMetricsConstants.Pack.MetricTagsTypeKey, out var metricType);

            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(context))
            {
                builder.Append(context);
                builder.Append('.');
            }

            builder.Append(name.Replace(' ', '_'));

            if (!string.IsNullOrWhiteSpace(metricType))
            {
                builder.Append('.');
                builder.Append(metricType);
            }

            tagsDictionary.TryGetValue(StatsDFormatterConstants.ItemTagName, out var itemTag);
            if (!string.IsNullOrWhiteSpace(itemTag))
            {
                builder.Append('.');
                builder.Append(itemTag);
            }

            builder.Append('.');
            builder.Append(field);

            var key = builder.ToString();
            var statsDMetricType = StatsDSyntax.FormatMetricType(metricType);

            if (!StatsDSyntax.CanBeSampled(statsDMetricType))
            {
                Points.Enqueue(new StatsDPoint(key, value, statsDMetricType, null, tags, serializer, timestamp));
                return;
            }

            tagsDictionary.TryGetValue(StatsDFormatterConstants.SampleRateTagName, out var tagSampleRateStr);
            var hasTagSampleRate = double.TryParse(tagSampleRateStr, out var tagSampleRate);

            var sampler = _samplers.GetOrAdd(key, _ => new StatsDSampler(_options.DefaultSampleRate));

            if (hasTagSampleRate)
                sampler.SampleRate = tagSampleRate;

            if(sampler.Sample(StatsDSyntax.FormatTimestamp(timestamp)))
                Points.Enqueue(new StatsDPoint(key, value, statsDMetricType, sampler.SampleRate, tags, serializer, timestamp));
        }

        public void Add(
            string context, 
            string name, 
            IEnumerable<string> columns, 
            IEnumerable<object> values,
            MetricTags tags, 
            IStatsDMetricStringSerializer serializer, 
            DateTime? timestamp)
        {
            var fields = columns
                .Zip(values, (column, data) => new { column, data })
                .ToDictionary(pair => pair.column, pair => pair.data);

            foreach (var kvp in fields)
            {
                Add(context, name, kvp.Key, kvp.Value, tags, serializer, timestamp);
            }
        }

        public async Task WriteAsync(Stream stream)
        {
            if (stream == null)
                return;

            var result = new List<string>();
            while (Points.TryDequeue(out var point))
                result.Add(point.Write(_options));

#if NETSTANDARD2_0
            using var writer = new StreamWriter(stream);
#elif NETSTANDARD2_1
            await using var writer = new StreamWriter(stream);
#endif
            await writer.WriteAsync(string.Join("\n", result));
        }
    }
}
