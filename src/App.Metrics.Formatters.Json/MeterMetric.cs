// <copyright file="MeterMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Formatters.Json
{
    /// <summary>
    ///     <para>
    ///         Meter metric types are increment-only counters that also measure the rate of events over time.
    ///     </para>
    ///     <para>
    ///         The mean rate is recorded, but is only generally used for trivia as it represents the total rate for the
    ///         lifetime of your application, not offering a sense of recency.
    ///     </para>
    ///     <para>
    ///         Other than the mean rate, meters also are recorded using three different exponentially-weighted moving average
    ///         rates, 1 min, 5 min and 15 min moving averages which do provide a sense of recency.
    ///     </para>
    ///     <para>
    ///         Meters also provide the ability to track the rate for each item in a finite set, as well as tracking a per item
    ///         rate the overall percentage is also recorded. This is useful for example if we needed to track the total
    ///         number of emails sent but also the rate at which each type of email is sent.
    ///     </para>
    /// </summary>
    /// <seealso cref="MetricBase" />
    public sealed class MeterMetric : MetricBase
    {
        public long Count { get; set; }

        public double FifteenMinuteRate { get; set; }

        public double FiveMinuteRate { get; set; }

        public IEnumerable<SetItem> Items { get; set; } = Enumerable.Empty<SetItem>();

        public double MeanRate { get; set; }

        public double OneMinuteRate { get; set; }

        public string RateUnit { get; set; }

        public sealed class SetItem
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