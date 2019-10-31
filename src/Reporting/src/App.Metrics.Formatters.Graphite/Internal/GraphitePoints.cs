// <copyright file="GraphitePoints.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace App.Metrics.Formatters.Graphite.Internal
{
    internal class GraphitePoints
    {
        private readonly List<GraphitePoint> _points = new List<GraphitePoint>();

        public void Add(GraphitePoint point)
        {
            if (point == null)
            {
                return;
            }

            _points.Add(point);
        }

        public async ValueTask WriteAsync(TextWriter textWriter, bool writeTimestamp = true)
        {
            if (textWriter == null)
            {
                return;
            }

            var points = _points.ToList();

            foreach (var point in points)
            {
                await point.WriteAsync(textWriter, writeTimestamp);
            }
        }
    }
}