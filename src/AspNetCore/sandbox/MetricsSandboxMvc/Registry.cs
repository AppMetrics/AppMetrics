// <copyright file="Registry.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics;
using App.Metrics.Histogram;

namespace MetricsSandboxMvc
{
    public static class Registry
    {
#pragma warning disable SA1401 // Fields must be private

        public static HistogramOptions One = new HistogramOptions
                                             {
                                                 Name = "test1",
                                                 MeasurementUnit = Unit.Bytes,
                                                 Context = "test"
                                             };

        public static HistogramOptions Three = new HistogramOptions
        {
                                                   Name = "test3",
                                                   MeasurementUnit = Unit.Bytes,
                                                   Context = "test"
                                               };

        public static HistogramOptions Two = new HistogramOptions
                                             {
                                                 Name = "test2",
                                                 MeasurementUnit = Unit.Bytes,
                                                 Context = "test"
                                             };
#pragma warning restore SA1401 // Fields must be private
    }
}