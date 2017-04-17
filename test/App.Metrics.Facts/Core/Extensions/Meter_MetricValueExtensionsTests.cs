using System.Collections.Generic;
using App.Metrics.Core.Extensions;
using App.Metrics.Meter;
using App.Metrics.Reporting;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Core.Extensions
{
    // ReSharper disable InconsistentNaming
    public class Meter_MetricValueExtensionsTests
        // ReSharper restore InconsistentNaming
    {
        private static readonly MetricValueDataKeys DataKeys = new MetricValueDataKeys();

        [Fact]
        public void meter_can_use_custom_data_keys_and_should_provide_corresponding_values()
        {
            // Arrange
            var value = new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds);
            var dataKeys = new MetricValueDataKeys(
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
        public void meter_default_data_keys_should_provide_corresponding_values()
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