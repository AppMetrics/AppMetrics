// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace App.Metrics.Extensions.Reporting.InfluxDB.Client
{
    public class LineProtocolPayload
    {
        private readonly List<LineProtocolPoint> _points = new List<LineProtocolPoint>();

        public void Add(LineProtocolPoint point)
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