// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;

namespace App.Metrics
{
    public sealed class Meter : Metric
    {
        public long Count { get; set; }

        public double FifteenMinuteRate { get; set; }

        public double FiveMinuteRate { get; set; }

        public IEnumerable<SetItem> Items { get; set; } = Enumerable.Empty<SetItem>();

        public double MeanRate { get; set; }

        public double OneMinuteRate { get; set; }

        public string RateUnit { get; set; }

        public class SetItem
        {
            public long Count { get; set; }

            public double FifteenMinuteRate { get; set; }

            public double FiveMinuteRate { get; set; }

            public string Item { get; set; }

            public double MeanRate { get; set; }

            public double OneMinuteRate { get; set; }

            public double Percent { get; set; }
        }
    }
}