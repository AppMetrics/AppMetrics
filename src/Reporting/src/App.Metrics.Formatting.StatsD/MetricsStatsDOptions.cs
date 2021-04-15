// <copyright file="MetricsStatsDOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatting.StatsD.Internal;

namespace App.Metrics.Formatting.StatsD
{
    /// <summary>
    ///     Provides programmatic configuration for GrafanaCloud Hosted Metrics format in the App Metrics framework.
    /// </summary>
    public class MetricsStatsDOptions
    {
        private double _defaultSampleRate;

        public MetricsStatsDOptions()
        {
            MetricNameFormatter = StatsDFormatterConstants.Defaults.MetricPointTextWriter;
            DefaultSampleRate = 1.0;
            TagMarker = '#';
        }

        public double DefaultSampleRate
        {
            get => _defaultSampleRate;
            set
            {
                if (value < 0.0 || value > 1.0)
                {
                    throw new IndexOutOfRangeException("SampleRate must be in the range of 0.0 and 1.0 inclusive.");
                }

                _defaultSampleRate = value;
            }
        }

        public IStatsDMetricStringSerializer MetricNameFormatter { get; set; }

        public char TagMarker { get; set; }
    }
}