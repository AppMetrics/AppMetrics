using System.Collections.Generic;
using App.Metrics.Core.Extensions;
using App.Metrics.Core.Internal;
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
        [Fact]
        public void meter_can_use_custom_data_keys_and_should_provide_corresponding_values()
        {
            // Arrange
            var value = new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds);
            var data = new Dictionary<string, object>();
            var customDataKeys = new Dictionary<MeterValueDataKeys, string>
                                 {
                                     { MeterValueDataKeys.Rate1M, "1_min_rate" },
                                     { MeterValueDataKeys.RateMean, "mean_rate" }
                                 };

            // Act
            value.AddMeterValues(data, customDataKeys);

            // Assert
            data.ContainsKey(Constants.DataKeyMapping.Meter[MeterValueDataKeys.RateMean]).Should().BeFalse();
            data["mean_rate"].Should().Be(2.0);
            data.ContainsKey(Constants.DataKeyMapping.Meter[MeterValueDataKeys.Rate1M]).Should().BeFalse();
            data["1_min_rate"].Should().Be(3.0);
        }

        [Fact]
        public void meter_default_data_keys_should_provide_corresponding_values()
        {
            // Arrange
            var value = new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds);
            var data = new Dictionary<string, object>();

            // Act
            value.AddMeterValues(data);

            // Assert
            data[Constants.DataKeyMapping.Meter[MeterValueDataKeys.Count]].Should().Be(1L);
            data[Constants.DataKeyMapping.Meter[MeterValueDataKeys.RateMean]].Should().Be(2.0);
            data[Constants.DataKeyMapping.Meter[MeterValueDataKeys.Rate1M]].Should().Be(3.0);
            data[Constants.DataKeyMapping.Meter[MeterValueDataKeys.Rate5M]].Should().Be(4.0);
            data[Constants.DataKeyMapping.Meter[MeterValueDataKeys.Rate15M]].Should().Be(5.0);
        }
    }
}