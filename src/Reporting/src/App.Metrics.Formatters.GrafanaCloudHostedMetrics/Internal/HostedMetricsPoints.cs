// <copyright file="HostedMetricsPoints.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace App.Metrics.Formatters.GrafanaCloudHostedMetrics.Internal
{
    internal class HostedMetricsPoints
    {
        private readonly List<HostedMetricsPoint> _points = new List<HostedMetricsPoint>();

        public void Add(HostedMetricsPoint point)
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
            writer.WriteStartArray();

            foreach (var point in points)
            {
                point.Write(writer, writeTimestamp);
            }

            writer.WriteEndArray();
        }
    }
}