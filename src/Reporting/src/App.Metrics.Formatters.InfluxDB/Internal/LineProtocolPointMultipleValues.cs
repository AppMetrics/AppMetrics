// <copyright file="LineProtocolPointMultipleValues.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace App.Metrics.Formatters.InfluxDB.Internal
{
    /// <summary>
    /// Represents a line procol point with multiple fields.
    /// </summary>
    internal class LineProtocolPointMultipleValues : LineProtocolPointBase, ILineProtocolPoint
    {
        public LineProtocolPointMultipleValues(
            string measurement,
            IEnumerable<string> fieldsNames,
            IEnumerable<object> fieldsValues,
            MetricTags tags,
            DateTime? utcTimestamp = null)
            : base(measurement, tags, utcTimestamp)
        {
            if (fieldsNames == null || !fieldsNames.Any())
            {
                throw new ArgumentException("At least one field must be specified");
            }

            if (fieldsNames.Any(f => string.IsNullOrEmpty(f)))
            {
                throw new ArgumentException("Fields must have non-empty names");
            }

            FieldsNames = fieldsNames;
            FieldsValues = fieldsValues;
        }

        public IEnumerable<string> FieldsNames { get; }

        public IEnumerable<object> FieldsValues { get; }

        public async ValueTask WriteAsync(TextWriter textWriter, bool writeTimestamp = true)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            await WriteCommonAsync(textWriter);

            var fieldDelim = ' ';

            using (var nameEnumerator = FieldsNames.GetEnumerator())
            {
                using var valueEnumerator = FieldsValues.GetEnumerator();
                while (nameEnumerator.MoveNext() && valueEnumerator.MoveNext())
                {
                    var name = nameEnumerator.Current;
                    var value = valueEnumerator.Current;

                    await textWriter.WriteAsync(fieldDelim);
                    fieldDelim = ',';
                    await textWriter.WriteAsync(LineProtocolSyntax.EscapeName(name));
                    await textWriter.WriteAsync('=');
                    await textWriter.WriteAsync(LineProtocolSyntax.FormatValue(value));
                }
            }

            if (!writeTimestamp)
            {
                return;
            }

            await WriteTimestampAsync(textWriter);
        }
    }
}