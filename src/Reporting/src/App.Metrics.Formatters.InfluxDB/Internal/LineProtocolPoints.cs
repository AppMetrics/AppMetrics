// <copyright file="LineProtocolPoints.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace App.Metrics.Formatters.InfluxDB.Internal
{
    internal class LineProtocolPoints
    {
        private readonly List<ILineProtocolPoint> _points = new List<ILineProtocolPoint>();

        public void Add(ILineProtocolPoint point)
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

            foreach (var point in points)
            {
                point.Write(textWriter, writeTimestamp);
                textWriter.Write('\n');
            }
        }
    }
}
