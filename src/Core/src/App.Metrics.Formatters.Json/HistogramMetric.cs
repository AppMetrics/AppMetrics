// <copyright file="HistogramMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.ReservoirSampling;

namespace App.Metrics.Formatters.Json
{
    /// <summary>
    ///     <para>
    ///         Histogram metric types track the statistical distribution of a set of values. They allow you to measure the
    ///         min, mean, max, standard deviation of values and also quantiles such as the median, 95th percentile, 98th
    ///         percentile etc.
    ///     </para>
    ///     <para>
    ///         To avoid unbound memory usage, the histogram values are genreated from a <see cref="IReservoir" /> of values.
    ///         This is done by sampling the data as it goes through by maintaining a small manageable  reservoir which is
    ///         statistically representative of the data stream as a whole, allowing us to quickly calculate the quantiles
    ///         which are valid approximations of the actual quantiles. https://en.wikipedia.org/wiki/Reservoir_sampling
    ///     </para>
    ///     <para>
    ///         Histograms also allow us to track user values, where for all user values provided the min, max and last user
    ///         value values is recorded.
    ///     </para>
    /// </summary>
    public sealed class HistogramMetric : MetricBase
    {
        public long Count { get; set; }

        public double Sum { get; set; }

        public string LastUserValue { get; set; }

        public double LastValue { get; set; }

        public double Max { get; set; }

        public string MaxUserValue { get; set; }

        public double Mean { get; set; }

        public double Median { get; set; }

        public double Min { get; set; }

        public string MinUserValue { get; set; }

        public double Percentile75 { get; set; }

        public double Percentile95 { get; set; }

        public double Percentile98 { get; set; }

        public double Percentile99 { get; set; }

        public double Percentile999 { get; set; }

        public int SampleSize { get; set; }

        public double StdDev { get; set; }
    }
}