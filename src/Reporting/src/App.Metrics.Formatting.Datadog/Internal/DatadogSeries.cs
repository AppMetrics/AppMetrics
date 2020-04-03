// <copyright file="DatadogPoints.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace App.Metrics.Formatting.Datadog.Internal
{
    internal class DatadogSeries
    {
        private readonly List<DatadogPoint> _points = new List<DatadogPoint>();

        public void Add(DatadogPoint point)
        {
            if (point == null)
            {
                return;
            }

            _points.Add(point);
        }

        public async Task WriteAsync(Stream stream, bool writeTimestamp = true)
        {
            if (stream == null)
            {
                return;
            }

            var points = _points.ToList();

            await using var writer = new Utf8JsonWriter(stream);
            
            writer.WriteStartObject();
            writer.WritePropertyName("series");
            writer.WriteStartArray();

            foreach (var point in points)
            {
                await point.Write(writer, writeTimestamp);
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}