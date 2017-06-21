// <copyright file="CustomAsciiMetricPayload.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;

namespace App.Metrics.Formatters.Ascii.Facts.TestHelpers
{
    public class CustomAsciiMetricPayload
    {
        private readonly List<CustomAsciiMetricPoint> _points = new List<CustomAsciiMetricPoint>();

        public void Add(CustomAsciiMetricPoint point)
        {
            if (point == null)
            {
                return;
            }

            _points.Add(point);
        }

        public void Format(TextWriter textWriter)
        {
            if (textWriter == null)
            {
                return;
            }

            foreach (var point in _points)
            {
                point.Format(textWriter);
                textWriter.Write('\n');
            }
        }
    }
}