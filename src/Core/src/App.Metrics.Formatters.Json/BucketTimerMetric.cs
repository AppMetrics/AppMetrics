// <copyright file="TimerMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.Formatters.Json
{
    /// <summary>
    ///     <para>
    ///         Timer metric types are essentially a special case of <see cref="Histogram" />
    ///     </para>
    ///     <para>
    ///         As well as providing a <see cref="Histogram" /> of the duration of a type of event, timers also provide a
    ///         <see cref="MeterMetric" /> of the rate of the events occurrence.
    ///     </para>
    ///     <para>
    ///         Like <see cref="Histogram" />s, timers also allow us to track user values, where for all user values provided
    ///         the min, max and last user value values is recorded.
    ///     </para>
    /// </summary>
    /// <seealso cref="MetricBase" />
    public sealed class BucketTimerMetric : MetricBase
    {
        public long ActiveSessions { get; set; }

        public long Count { get; set; }

        public string DurationUnit { get; set; }

        public BucketHistogramData Histogram { get; set; }

        public RateData Rate { get; set; }

        public string RateUnit { get; set; }

        public sealed class BucketHistogramData
        {
            public IDictionary<double, double> Buckets { get; set; }

            public double Sum { get; set; }
        }

        public sealed class RateData
        {
            public double FifteenMinuteRate { get; set; }

            public double FiveMinuteRate { get; set; }

            public double MeanRate { get; set; }

            public double OneMinuteRate { get; set; }
        }
    }
}