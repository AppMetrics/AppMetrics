// <copyright file="Histogram_MetricValueExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Formatters;
using App.Metrics.Histogram;
using App.Metrics.Internal;
using App.Metrics.Serialization;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Histogram
{
    // ReSharper disable InconsistentNaming
    public class Histogram_MetricValueExtensionsTests
        // ReSharper restore InconsistentNaming
    {
        private static readonly GeneratedMetricNameMapping DataKeys = new GeneratedMetricNameMapping();

        [Fact]
        public void Histogram_can_use_custom_data_keys_and_should_provide_corresponding_values()
        {
            // Arrange
            var value = new HistogramValue(1, 1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);
            var data = new Dictionary<string, object>();
            var dataKeys = new GeneratedMetricNameMapping(
                histogram: new Dictionary<HistogramValueDataKeys, string>
                           {
                               { HistogramValueDataKeys.UserLastValue, "userLastValue" },
                               { HistogramValueDataKeys.UserMinValue, "userMinValue" },
                               { HistogramValueDataKeys.UserMaxValue, "userMaxValue" },
                               { HistogramValueDataKeys.P75, "75th_percentile" }
                           });

            // Act
            value.AddHistogramValues(data, dataKeys.Histogram);

            // Assert
            data.ContainsKey(DataKeys.Histogram[HistogramValueDataKeys.UserLastValue]).Should().BeFalse();
            data["userLastValue"].Should().Be("3");
            data.ContainsKey(DataKeys.Histogram[HistogramValueDataKeys.UserMaxValue]).Should().BeFalse();
            data["userMaxValue"].Should().Be("5");
            data.ContainsKey(DataKeys.Histogram[HistogramValueDataKeys.UserMinValue]).Should().BeFalse();
            data["userMinValue"].Should().Be("8");
            data.ContainsKey(DataKeys.Histogram[HistogramValueDataKeys.P75]).Should().BeFalse();
            data["75th_percentile"].Should().Be(11.0);
        }

        [Fact]
        public void Histogram_default_data_keys_should_provide_corresponding_values()
        {
            // Arrange
            var value = new HistogramValue(1, 1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);
            var data = new Dictionary<string, object>();

            // Act
            value.AddHistogramValues(data, DataKeys.Histogram);

            // Assert
            data[DataKeys.Histogram[HistogramValueDataKeys.Count]].Should().Be(1L);
            data[DataKeys.Histogram[HistogramValueDataKeys.Sum]].Should().Be(1.0);
            data[DataKeys.Histogram[HistogramValueDataKeys.LastValue]].Should().Be(2.0);
            data[DataKeys.Histogram[HistogramValueDataKeys.UserLastValue]].Should().Be("3");
            data[DataKeys.Histogram[HistogramValueDataKeys.Max]].Should().Be(4.0);
            data[DataKeys.Histogram[HistogramValueDataKeys.UserMaxValue]].Should().Be("5");
            data[DataKeys.Histogram[HistogramValueDataKeys.Mean]].Should().Be(6.0);
            data[DataKeys.Histogram[HistogramValueDataKeys.Min]].Should().Be(7.0);
            data[DataKeys.Histogram[HistogramValueDataKeys.UserMinValue]].Should().Be("8");
            data[DataKeys.Histogram[HistogramValueDataKeys.StdDev]].Should().Be(9.0);
            data[DataKeys.Histogram[HistogramValueDataKeys.Median]].Should().Be(10.0);
            data[DataKeys.Histogram[HistogramValueDataKeys.P75]].Should().Be(11.0);
            data[DataKeys.Histogram[HistogramValueDataKeys.P95]].Should().Be(12.0);
            data[DataKeys.Histogram[HistogramValueDataKeys.P98]].Should().Be(13.0);
            data[DataKeys.Histogram[HistogramValueDataKeys.P99]].Should().Be(14.0);
            data[DataKeys.Histogram[HistogramValueDataKeys.P999]].Should().Be(15.0);
            data[DataKeys.Histogram[HistogramValueDataKeys.Samples]].Should().Be(16);
        }
    }
}