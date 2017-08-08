// <copyright file="MetricsTextPoints.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace App.Metrics.Formatters.Ascii.Internal
{
    internal class MetricsTextPoints
    {
        private readonly List<MetricsTextPoint> _points = new List<MetricsTextPoint>();

        public void Add(MetricsTextPoint textPoint)
        {
            if (textPoint == null)
            {
                return;
            }

            _points.Add(textPoint);
        }

        public void Write(TextWriter textWriter, string separator, int padding)
        {
            if (textWriter == null)
            {
                return;
            }

            var points = _points.ToList();

            foreach (var point in points)
            {
                point.Write(textWriter, separator, padding);
                textWriter.Write('\n');
            }
        }
    }
}
