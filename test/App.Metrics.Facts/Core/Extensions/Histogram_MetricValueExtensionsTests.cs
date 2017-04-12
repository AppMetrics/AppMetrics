using System.Collections.Generic;
using App.Metrics.Core;
using App.Metrics.Core.Extensions;
using App.Metrics.Core.Internal;
using App.Metrics.Histogram;
using App.Metrics.Reporting;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Core.Extensions
{
    // ReSharper disable InconsistentNaming
    public class Histogram_MetricValueExtensionsTests
        // ReSharper restore InconsistentNaming
    {
        [Fact]
        public void histogram_can_use_custom_data_keys_and_should_provide_corresponding_values()
        {
            // Arrange
            var value = new HistogramValue(1, 1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);
            var data = new Dictionary<string, object>();
            var customDataKeys = new Dictionary<HistogramDataKeys, string>
                                 {
                                     { HistogramDataKeys.UserLastValue, "userLastValue" },
                                     { HistogramDataKeys.UserMinValue, "userMinValue" },
                                     { HistogramDataKeys.UserMaxValue, "userMaxValue" },
                                     { HistogramDataKeys.P75, "75th_percentile" }
                                 };

            // Act
            value.AddHistogramValues(data, customDataKeys);

            // Assert
            data.ContainsKey(Constants.DataKeyMapping.Histogram[HistogramDataKeys.UserLastValue]).Should().BeFalse();
            data["userLastValue"].Should().Be("3");
            data.ContainsKey(Constants.DataKeyMapping.Histogram[HistogramDataKeys.UserMaxValue]).Should().BeFalse();
            data["userMaxValue"].Should().Be("5");
            data.ContainsKey(Constants.DataKeyMapping.Histogram[HistogramDataKeys.UserMinValue]).Should().BeFalse();
            data["userMinValue"].Should().Be("8");
            data.ContainsKey(Constants.DataKeyMapping.Histogram[HistogramDataKeys.P75]).Should().BeFalse();
            data["75th_percentile"].Should().Be(11.0);
        }

        [Fact]
        public void histogram_default_data_keys_should_provide_corresponding_values()
        {
            // Arrange
            var value = new HistogramValue(1, 1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);
            var data = new Dictionary<string, object>();

            // Act
            value.AddHistogramValues(data);

            // Assert
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.Count]].Should().Be(1L);
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.Sum]].Should().Be(1.0);
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.LastValue]].Should().Be(2.0);
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.UserLastValue]].Should().Be("3");
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.Max]].Should().Be(4.0);
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.UserMaxValue]].Should().Be("5");
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.Mean]].Should().Be(6.0);
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.Min]].Should().Be(7.0);
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.UserMinValue]].Should().Be("8");
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.StdDev]].Should().Be(9.0);
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.Median]].Should().Be(10.0);
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.P75]].Should().Be(11.0);
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.P95]].Should().Be(12.0);
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.P98]].Should().Be(13.0);
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.P99]].Should().Be(14.0);
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.P999]].Should().Be(15.0);
            data[Constants.DataKeyMapping.Histogram[HistogramDataKeys.Samples]].Should().Be(16);
        }
    }
}