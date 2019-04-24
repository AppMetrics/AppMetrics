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
            using (var memoryStream = new MemoryStream())
            {
                await Write(memoryStream, metrics, newLine);

                return Encoding.GetString(memoryStream.ToArray());
            }
        }

        public static async Task Write(Stream destination, IEnumerable<MetricFamily> metrics, NewLineFormat newLine)
        {
            var metricFamilies = metrics.ToArray();
            using (var streamWriter = new StreamWriter(destination, Encoding, bufferSize: 1024, leaveOpen: true) { NewLine = GetNewLineChar(newLine) })
            {
                foreach (var metricFamily in metricFamilies)
                {
                    WriteFamily(streamWriter, metricFamily);
                }

                await streamWriter.FlushAsync();
            }
        }

        private static void WriteFamily(StreamWriter streamWriter, MetricFamily metricFamily)
        {
            streamWriter.Write("# HELP ");
            streamWriter.Write(metricFamily.name);
            streamWriter.Write(' ');
            streamWriter.WriteLine(metricFamily.help);

            streamWriter.Write("# TYPE ");
            streamWriter.Write(metricFamily.name);
            streamWriter.Write(' ');
            streamWriter.WriteLine(ToString(metricFamily.type));

            foreach (var metric in metricFamily.metric)
            {
                WriteMetric(streamWriter, metricFamily, metric);
                streamWriter.WriteLine();
            }
        }

        private static void WriteMetric(StreamWriter streamWriter, MetricFamily family, Metric metric)
        {
            var familyName = family.name;

            if (metric.gauge != null)
            {
                WriteSimpleValue(streamWriter, familyName, metric.gauge.value, metric.label);
            }
            else if (metric.counter != null)
            {
                WriteSimpleValue(streamWriter, familyName, metric.counter.value, metric.label);
            }
            else if (metric.summary != null)
            {
                WriteSimpleValue(streamWriter, familyName, metric.summary.sample_sum, metric.label, "_sum");
                WriteSimpleValue(streamWriter, familyName, metric.summary.sample_count, metric.label, "_count");

                foreach (var quantileValuePair in metric.summary.quantile)
                {
                    var quantile = double.IsPositiveInfinity(quantileValuePair.quantile) ? "+Inf" : quantileValuePair.quantile.ToString(CultureInfo.InvariantCulture);

                    WriteSimpleValue(
                        streamWriter,
                        familyName,
                        quantileValuePair.value,
                        metric.label.Concat(new[] { new LabelPair { name = "quantile", value = quantile } }));
                }
            }
            else if (metric.histogram != null)
            {
                WriteSimpleValue(streamWriter, familyName, metric.histogram.sample_sum, metric.label, "_sum");
                WriteSimpleValue(streamWriter, familyName, metric.histogram.sample_count, metric.label, "_count");
                foreach (var bucket in metric.histogram.bucket)
                {
                    var value = double.IsPositiveInfinity(bucket.upper_bound) ? "+Inf" : bucket.upper_bound.ToString(CultureInfo.InvariantCulture);

                    WriteSimpleValue(
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

        private static void WriteSimpleValue(StreamWriter writer, string family, double value, IEnumerable<LabelPair> labels, string namePostfix = null)
        {
            writer.Write(family);
            if (namePostfix != null)
            {
                writer.Write(namePostfix);
            }

            bool any = false;
            foreach (var l in labels)
            {
                writer.Write(any ? ',' : '{');

                writer.Write(l.name);
                writer.Write("=\"");
                writer.Write(l.value);
                writer.Write('"');

                any = true;
            }

            if (any)
            {
                writer.Write('}');
            }

            writer.Write(' ');
            writer.WriteLine(value.ToString(CultureInfo.InvariantCulture));
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
