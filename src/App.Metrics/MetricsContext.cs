// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;

namespace App.Metrics
{
    public sealed class MetricsContext
    {
        public string Context { get; set; }

        public IEnumerable<Counter> Counters { get; set; } = Enumerable.Empty<Counter>();

        public IEnumerable<Gauge> Gauges { get; set; } = Enumerable.Empty<Gauge>();

        public IEnumerable<Histogram> Histograms { get; set; } = Enumerable.Empty<Histogram>();

        public IEnumerable<Meter> Meters { get; set; } = Enumerable.Empty<Meter>();

        public IEnumerable<Timer> Timers { get; set; } = Enumerable.Empty<Timer>();
    }
}