// <copyright file="TestMetricPoint.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace App.Metrics.Reporting.Facts.TestHelpers
{
    public class TestMetricPoint
    {
        private static readonly Dictionary<Type, Func<object, string>> Formatters = new Dictionary<Type, Func<object, string>>
                                                                                    {
                                                                                        { typeof(sbyte), FormatInteger },
                                                                                        { typeof(byte), FormatInteger },
                                                                                        { typeof(short), FormatInteger },
                                                                                        { typeof(ushort), FormatInteger },
                                                                                        { typeof(int), FormatInteger },
                                                                                        { typeof(uint), FormatInteger },
                                                                                        { typeof(long), FormatInteger },
                                                                                        { typeof(ulong), FormatInteger },
                                                                                        { typeof(float), FormatFloat },
                                                                                        { typeof(double), FormatFloat },
                                                                                        { typeof(decimal), FormatFloat },
                                                                                        { typeof(bool), FormatBoolean },
                                                                                        { typeof(TimeSpan), FormatTimespan }
                                                                                    };

        public TestMetricPoint(
            string measurement,
            IReadOnlyDictionary<string, object> fields,
            MetricTags tags,
            DateTime? utcTimestamp = null)
        {
            if (string.IsNullOrEmpty(measurement))
            {
                throw new ArgumentException("A measurement name must be specified");
            }

            if (fields == null || fields.Count == 0)
            {
                throw new ArgumentException("At least one field must be specified");
            }

            if (fields.Any(f => string.IsNullOrEmpty(f.Key)))
            {
                throw new ArgumentException("Fields must have non-empty names");
            }

            if (utcTimestamp != null && utcTimestamp.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Timestamps must be specified as UTC");
            }

            Measurement = measurement;
            Fields = fields;
            Tags = tags;
            UtcTimestamp = utcTimestamp;
        }

        public IReadOnlyDictionary<string, object> Fields { get; }

        public string Measurement { get; }

        public MetricTags Tags { get; }

        public DateTime? UtcTimestamp { get; }

        public static string FormatValue(object value)
        {
            var v = value ?? string.Empty;

            return Formatters.TryGetValue(v.GetType(), out Func<object, string> format)
                ? format(v)
                : FormatString(v.ToString());
        }

        public void Format(StringBuilder sb)
        {
            sb.Append((string)Measurement);

            if (Tags.Count > 0)
            {
                for (var i = 0; i < Tags.Count; i++)
                {
                    sb.Append(' ');
                    sb.Append((string)Tags.Keys[i]);
                    sb.Append('=');
                    sb.Append((string)Tags.Values[i]);
                }
            }

            var fieldDelim = ' ';

            foreach (var f in Fields)
            {
                sb.Append(fieldDelim);
                fieldDelim = ' ';
                sb.Append((string)f.Key);
                sb.Append('=');
                sb.Append((string)FormatValue(f.Value));
            }
        }

        private static string FormatBoolean(object b) { return (bool)b ? "t" : "f"; }

        private static string FormatFloat(object f) { return ((IFormattable)f).ToString(null, CultureInfo.InvariantCulture); }

        private static string FormatInteger(object i) { return ((IFormattable)i).ToString(null, CultureInfo.InvariantCulture) + "i"; }

        private static string FormatString(string s) { return "\"" + s.Replace("\"", "\\\"") + "\""; }

        private static string FormatTimespan(object ts) { return ((TimeSpan)ts).TotalMilliseconds.ToString(CultureInfo.InvariantCulture); }
    }
}