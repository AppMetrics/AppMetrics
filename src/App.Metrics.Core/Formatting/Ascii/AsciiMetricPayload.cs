// <copyright file="AsciiMetricPayload.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace App.Metrics.Core.Formatting.Ascii
{
    // TODO: Remove in 2.0.0 - Add to separate formatting package
    public class AsciiMetricPayload
    {
        private readonly List<AsciiMetricPoint> _points = new List<AsciiMetricPoint>();

        public void Add(AsciiMetricPoint point)
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

            var points = _points.ToList();

            foreach (var point in points)
            {
                point.Format(textWriter);
                textWriter.Write('\n');
            }
        }
    }
}
