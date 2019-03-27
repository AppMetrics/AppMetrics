// <copyright file="MetricsInfluxDBLineProtocolOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.InfluxDB.Internal;

namespace App.Metrics.Formatters.InfluxDB
{
    /// <summary>
    ///     Provides programmatic configuration for InfluxDB's LineProtocole format in the App Metrics framework.
    /// </summary>
    public class MetricsInfluxDbLineProtocolOptions
    {
        public MetricsInfluxDbLineProtocolOptions()
        {
            MetricNameFormatter = InfluxDbFormatterConstants.LineProtocol.MetricNameFormatter;
        }

        public Func<string, string, string> MetricNameFormatter { get; set; }
    }
}
