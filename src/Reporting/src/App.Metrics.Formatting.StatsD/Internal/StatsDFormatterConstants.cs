// <copyright file="StatsDFormatterConstants.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Formatting.StatsD.Internal
{
    public class StatsDFormatterConstants
    {
        public static string ItemTagName = "item";
        public static string SampleRateTagName = "sampleRate";

        public static class Defaults
        {
            public static readonly IStatsDMetricStringSerializer MetricPointTextWriter = new DefaultStatsDMetricStringSerializer();
        }
    }
}