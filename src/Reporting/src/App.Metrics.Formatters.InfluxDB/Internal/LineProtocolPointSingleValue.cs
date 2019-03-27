// <copyright file="LineProtocolPointSingleValue.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;

namespace App.Metrics.Formatters.InfluxDB.Internal
{
    /// <summary>
    /// Represents a line protocol point with a single field.
    /// </summary>
    internal class LineProtocolPointSingleValue : LineProtocolPointBase, ILineProtocolPoint
    {
        public LineProtocolPointSingleValue(
            string measurement,
            string fieldName,
            object fieldValue,
            MetricTags tags,
            DateTime? utcTimestamp = null)
            : base(measurement, tags, utcTimestamp)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentException("Field name must be specified and be non-empty");
            }

            FieldName = fieldName;
            FieldValue = fieldValue;
        }

        public string FieldName { get; }

        public object FieldValue { get; }

        public void Write(TextWriter textWriter, bool writeTimestamp = true)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            WriteCommon(textWriter);

            textWriter.Write(' ');
            textWriter.Write(LineProtocolSyntax.EscapeName(FieldName));
            textWriter.Write('=');
            textWriter.Write(LineProtocolSyntax.FormatValue(FieldValue));

            if (!writeTimestamp)
            {
                return;
            }

            WriteTimestamp(textWriter);
        }
    }
}
