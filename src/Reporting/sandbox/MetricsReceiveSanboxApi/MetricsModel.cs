// <copyright file="MetricsModel.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace MetricsReceiveSanboxApi
{
#pragma warning disable SA1402 // File may only contain a single class
    // ReSharper disable ClassNeverInstantiated.Global
    public class MetricsModel
    {
        public MetricsModel() { Contexts = new List<MetricsContext>(); }

        public List<MetricsContext> Contexts { get; set; }

        public DateTime Timestamp { get; set; }
    }

    public class MetricsContext
    {
        public string Context { get; set; }

        public List<Gauge> Gauges { get; set; }

        public List<Counter> Counters { get; set; }
    }

    public class Gauge
    {
        public string Name { get; set; }

        public string Unit { get; set; }

        public double Value { get; set; }
    }

    public class Counter
    {
        public string Name { get; set; }

        public string Unit { get; set; }

        public long Count { get; set; }
    }

    // ReSharper restore ClassNeverInstantiated.Global
#pragma warning restore SA1402 // File may only contain a single class
}