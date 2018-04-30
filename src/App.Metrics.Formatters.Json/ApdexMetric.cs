// <copyright file="ApdexMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Formatters.Json
{
    public sealed class ApdexMetric : MetricBase
    {
        public int Frustrating { get; set; }

        public int SampleSize { get; set; }

        public int Satisfied { get; set; }

        public double Score { get; set; }

        public int Tolerating { get; set; }
    }
}