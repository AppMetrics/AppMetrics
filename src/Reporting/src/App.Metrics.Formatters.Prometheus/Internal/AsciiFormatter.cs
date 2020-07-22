// <copyright file="AsciiFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Metrics.Formatters.Prometheus.Internal
{
    internal static class AsciiFormatter
    {
        private static readonly UTF8Encoding Encoding = new UTF8Encoding(false);

        public static async Task<string> Format(IEnumerable<MetricFamily> metrics, NewLineFormat newLine)
        {
#if NETSTANDARD2_1
            await using var memoryStream = new MemoryStream();
#else
            using var memoryStream = new MemoryStream();
#endif
            await Write(memoryStream, metrics, newLine);

            return Encoding.GetString(memoryStream.ToArray());
        }

        public static async Task Write(Stream destination, IEnumerable<MetricFamily> metrics, NewLineFormat newLine)
        {
            var metricFamilies = metrics.ToArray();
#if NETSTANDARD2_1
            await using var streamWriter = new StreamWriter(destination, Encoding, bufferSize: 1024, leaveOpen: true) { NewLine = GetNewLineChar(newLine) };
#else
            using var streamWriter = new StreamWriter(destination, Encoding, bufferSize: 1024, leaveOpen: true) { NewLine = GetNewLineChar(newLine) };
#endif
            foreach (var metricFamily in metricFamilies)
            {
                await WriteFamily(streamWriter, metricFamily);
            }

            await streamWriter.FlushAsync();
        }

        private static async Task WriteFamily(StreamWriter streamWriter, MetricFamily metricFamily)
        {
            await streamWriter.WriteAsync("# HELP ");
            await streamWriter.WriteAsync(metricFamily.name);
            await streamWriter.WriteAsync(' ');
            await streamWriter.WriteLineAsync(metricFamily.help);

            await streamWriter.WriteAsync("# TYPE ");
            await streamWriter.WriteAsync(metricFamily.name);
            await streamWriter.WriteAsync(' ');
            await streamWriter.WriteLineAsync(ToString(metricFamily.type));

            foreach (var metric in metricFamily.metric)
            {
                await WriteMetric(streamWriter, metricFamily, metric);
                await streamWriter.WriteLineAsync();
            }
        }

        private static async Task WriteMetric(StreamWriter streamWriter, MetricFamily family, Metric metric)
        {
            var familyName = family.name;

            if (metric.gauge != null)
            {
                await WriteSimpleValue(streamWriter, familyName, metric.gauge.value, metric.label);
            }
            else if (metric.counter != null)
            {
                await WriteSimpleValue(streamWriter, familyName, metric.counter.value, metric.label);
            }
            else if (metric.summary != null)
            {
                await WriteSimpleValue(streamWriter, familyName, metric.summary.sample_sum, metric.label, "_sum");
                await WriteSimpleValue(streamWriter, familyName, metric.summary.sample_count, metric.label, "_count");

                foreach (var quantileValuePair in metric.summary.quantile)
                {
                    var quantile = double.IsPositiveInfinity(quantileValuePair.quantile) ? "+Inf" : quantileValuePair.quantile.ToString(CultureInfo.InvariantCulture);

                    await WriteSimpleValue(
                        streamWriter,
                        familyName,
                        quantileValuePair.value,
                        metric.label.Concat(new[] { new LabelPair { name = "quantile", value = quantile } }));
                }
            }
            else if (metric.histogram != null)
            {
                await WriteSimpleValue(streamWriter, familyName, metric.histogram.sample_sum, metric.label, "_sum");
                await WriteSimpleValue(streamWriter, familyName, metric.histogram.sample_count, metric.label, "_count");
                foreach (var bucket in metric.histogram.bucket)
                {
                    var value = double.IsPositiveInfinity(bucket.upper_bound) ? "+Inf" : bucket.upper_bound.ToString(CultureInfo.InvariantCulture);

                    await WriteSimpleValue(
                        streamWriter,
                        familyName,
                        bucket.cumulative_count,
                        metric.label.Concat(new[] { new LabelPair { name = "le", value = value } }),
                        "_bucket");
                }
            }
            else
            {
                // not supported
            }
        }

        private static async Task WriteSimpleValue(StreamWriter writer, string family, double value, IEnumerable<LabelPair> labels, string namePostfix = null)
        {
            await writer.WriteAsync(family);
            if (namePostfix != null)
            {
                await writer.WriteAsync(namePostfix);
            }

            bool any = false;
            foreach (var l in labels)
            {
                await writer.WriteAsync(any ? ',' : '{');

                await writer.WriteAsync(l.name);
                await writer.WriteAsync("=\"");
                await writer.WriteAsync(l.value);
                await writer.WriteAsync('"');

                any = true;
            }

            if (any)
            {
                await writer.WriteAsync('}');
            }

            await writer.WriteAsync(' ');
            await writer.WriteLineAsync(value.ToString(CultureInfo.InvariantCulture));
        }

        private static string GetNewLineChar(NewLineFormat newLine)
        {
            switch (newLine)
            {
                case NewLineFormat.Auto:
                    return Environment.NewLine;
                case NewLineFormat.Windows:
                    return "\r\n";
                case NewLineFormat.Unix:
                case NewLineFormat.Default:
                    return "\n";
                default:
                    throw new ArgumentOutOfRangeException(nameof(newLine), newLine, null);
            }
        }

        private static string ToString(MetricType type)
        {
            switch (type)
            {
                case MetricType.COUNTER:
                    return "counter";
                case MetricType.GAUGE:
                    return "gauge";
                case MetricType.SUMMARY:
                    return "summary";
                case MetricType.UNTYPED:
                    return "untyped";
                case MetricType.HISTOGRAM:
                    return "histogram";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}