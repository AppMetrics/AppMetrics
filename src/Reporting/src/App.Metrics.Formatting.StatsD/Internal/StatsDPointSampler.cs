// <copyright file="StatsDPointSampler.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
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
                                                                  AppMetricsConstants.Pack.MetricTagsUnitKey,
                                                                  AppMetricsConstants.Pack.MetricTagsUnitRateKey,
                                                                  AppMetricsConstants.Pack.MetricTagsUnitRateDurationKey,
                                                                  StatsDFormatterConstants.SampleRateTagName,
                                                                  StatsDFormatterConstants.ItemTagName
                                                              };

        private readonly ConcurrentDictionary<string, StatsDSampler> _samplers = new ConcurrentDictionary<string, StatsDSampler>();

        public StatsDPointSampler(MetricsStatsDOptions options) { Options = options; }

        public ConcurrentQueue<StatsDPoint> Points { get; } = new ConcurrentQueue<StatsDPoint>();
        public MetricsStatsDOptions Options { get; }

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
            {
                throw new ArgumentNullException(nameof(name), "Name could not be null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(field))
            {
                throw new ArgumentNullException(nameof(field), "Field could not be null or whitespace");
            }

            var tagsDictionary = metricTags.ToDictionary();
            tagsDictionary.TryGetValue(AppMetricsConstants.Pack.MetricTagsTypeKey, out var metricType);
            tagsDictionary.TryGetValue(StatsDFormatterConstants.ItemTagName, out var itemTag);

            var key = BuildKeyName(context, name, metricType, itemTag, field);

            var tags = tagsDictionary.Where(tag => !ExcludeTags.Contains(tag.Key)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var statsDMetricType = StatsDSyntax.FormatMetricType(metricType);
            if (metricType == AppMetricsConstants.Pack.ApdexMetricTypeValue && field != "score")
                statsDMetricType = StatsDSyntax.Count;

            if (!StatsDSyntax.CanBeSampled(statsDMetricType) || metricType == AppMetricsConstants.Pack.MeterMetricTypeValue)
            {
                Points.Enqueue(new StatsDPoint(key, value, statsDMetricType, null, tags, serializer, timestamp));
                return;
            }

            tagsDictionary.TryGetValue(StatsDFormatterConstants.SampleRateTagName, out var tagSampleRateStr);
            if (!double.TryParse(tagSampleRateStr, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.InvariantInfo, out var tagSampleRate))
            {
                tagSampleRate = Options.DefaultSampleRate;
            }

            var sampler = _samplers.GetOrAdd(key, _ => new StatsDSampler());

            if (sampler.ShouldSend(tagSampleRate))
            {
                Points.Enqueue(new StatsDPoint(key, value, statsDMetricType, tagSampleRate, tags, serializer, timestamp));
            }
        }

        private string BuildKeyName(string context, string name, string metricType, string itemTag, string field)
        {
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

            if (!string.IsNullOrWhiteSpace(itemTag))
            {
                builder.Append('.');
                builder.Append(itemTag);
            }

            builder.Append('.');
            builder.Append(field);

            return builder.ToString();
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
            var fields = columns.Zip(values, (column, data) => new { column, data }).ToDictionary(pair => pair.column, pair => pair.data);

            var tagsDictionary = tags.ToDictionary();
            tagsDictionary.TryGetValue(AppMetricsConstants.Pack.MetricTagsTypeKey, out var metricType);

            switch (metricType)
            {
                case "histogram":
                    Add(context, name, "value", fields["median"], tags, serializer, timestamp);
                    break;

                case "meter":
                    Add(context, name, "value", fields["count.meter"], tags, serializer, timestamp);
                    break;

                case "timer":
                    var value = fields["median"] switch
                    {
                        double d => Convert.ToInt64(d),
                        float f => Convert.ToInt64(f),
                        _ => (long) fields["median"]
                    };
                    var unit = tagsDictionary[AppMetricsConstants.Pack.MetricTagsUnitRateDurationKey].FromUnit();
                    value = unit.Convert(TimeUnit.Milliseconds, value);
                    Add(context, name, "value", value, tags, serializer, timestamp);
                    break;

                default:
                    foreach (var kvp in fields)
                    {
                        Add(context, name, kvp.Key, kvp.Value, tags, serializer, timestamp);
                    }
                    break;
            }
        }

        public async Task WriteAsync(Stream stream)
        {
            if (stream == null)
            {
                return;
            }

            var result = new List<string>();
            while (Points.TryDequeue(out var point))
            {
                result.Add(point.Write(Options));
            }

#if NETSTANDARD2_0
            using var writer = new StreamWriter(stream);
#elif NETSTANDARD2_1
            await using var writer = new StreamWriter(stream);
#endif
            await writer.WriteAsync(string.Join("\n", result));
        }
    }
}