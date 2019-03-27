// <copyright file="HostedMetricsPoints.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

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

        public void Write(TextWriter textWriter, bool writeTimestamp = true)
        {
            if (textWriter == null)
            {
                return;
            }

            var points = _points.ToList();

            using (JsonWriter writer = new JsonTextWriter(textWriter))
            {
                writer.WriteStartArray();

                foreach (var point in points)
                {
                    point.Write(writer, writeTimestamp);
                }

                writer.WriteEndArray();
            }
        }
    }
}