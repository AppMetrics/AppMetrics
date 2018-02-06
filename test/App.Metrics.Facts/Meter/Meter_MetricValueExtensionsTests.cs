// <copyright file="Meter_MetricValueExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Formatters;
using App.Metrics.Internal;
using App.Metrics.Meter;
using App.Metrics.Serialization;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Meter
{
    // ReSharper disable InconsistentNaming
    public class Meter_MetricValueExtensionsTests
        // ReSharper restore InconsistentNaming
    {
        private static readonly GeneratedMetricNameMapping DataKeys = new GeneratedMetricNameMapping();

        [Fact]
        public void Meter_can_use_custom_data_keys_and_should_provide_corresponding_values()
        {
            // Arrange
            var value = new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds);
            var dataKeys = new GeneratedMetricNameMapping(
                meter: new Dictionary<MeterValueDataKeys, string>
                       {
                           { MeterValueDataKeys.Rate1M, "1_min_rate" },
                           { MeterValueDataKeys.RateMean, "mean_rate" }
                       });

            // Act
            value.AddMeterValues(out IDictionary<string, object> data, dataKeys.Meter);

            // Assert
            data.ContainsKey(DataKeys.Meter[MeterValueDataKeys.RateMean]).Should().BeFalse();
            data["mean_rate"].Should().Be(2.0);
            data.ContainsKey(DataKeys.Meter[MeterValueDataKeys.Rate1M]).Should().BeFalse();
            data["1_min_rate"].Should().Be(3.0);
        }

        [Fact]
        public void Meter_default_data_keys_should_provide_corresponding_values()
        {
            // Arrange
            var value = new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds);

            // Act
            value.AddMeterValues(out IDictionary<string, object> data, DataKeys.Meter);

            // Assert
            data[DataKeys.Meter[MeterValueDataKeys.Count]].Should().Be(1L);
            data[DataKeys.Meter[MeterValueDataKeys.RateMean]].Should().Be(2.0);
            data[DataKeys.Meter[MeterValueDataKeys.Rate1M]].Should().Be(3.0);
            data[DataKeys.Meter[MeterValueDataKeys.Rate5M]].Should().Be(4.0);
            data[DataKeys.Meter[MeterValueDataKeys.Rate15M]].Should().Be(5.0);
        }
    }
}