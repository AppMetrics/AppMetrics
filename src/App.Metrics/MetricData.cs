// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;

namespace App.Metrics
{
    public sealed class MetricData
    {
        public IEnumerable<MetricsContext> Contexts { get; set; } = new MetricsContext[0];

        public IDictionary<string, string> Environment { get; set; }

        public DateTime Timestamp { get; set; }
    }
}