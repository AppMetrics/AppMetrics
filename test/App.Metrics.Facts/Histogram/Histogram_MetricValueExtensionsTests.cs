// <copyright file="Histogram_MetricValueExtensionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Histogram;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Histogram
{
    // ReSharper disable InconsistentNaming
    public class Histogram_MetricValueExtensionsTests
        // ReSharper restore InconsistentNaming
    {
        private static readonly MetricFields Fields = new MetricFields();
        private readonly Func<HistogramValue> _histogramValue = () => new HistogramValue(1, 1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);

        [Fact]
        public void Histogram_can_use_custom_field_names_and_should_provide_corresponding_values()
        {
            // Arrange
            var value = _histogramValue();
            var data = new Dictionary<string, object>();
            var fields = new MetricFields();
            fields.Histogram.Set(HistogramFields.UserLastValue, "userLastValue");
            fields.Histogram.Set(HistogramFields.UserMinValue, "userMinValue");
            fields.Histogram.Set(HistogramFields.UserMaxValue, "userMaxValue");
            fields.Histogram.Set(HistogramFields.P75, "75th_percentile");

            // Act
            value.AddHistogramValues(data, fields.Histogram);

            // Assert
            data.ContainsKey(Fields.Histogram[HistogramFields.UserLastValue]).Should().BeFalse();
            data["userLastValue"].Should().Be("3");
            data.ContainsKey(Fields.Histogram[HistogramFields.UserMaxValue]).Should().BeFalse();
            data["userMaxValue"].Should().Be("5");
            data.ContainsKey(Fields.Histogram[HistogramFields.UserMinValue]).Should().BeFalse();
            data["userMinValue"].Should().Be("8");
            data.ContainsKey(Fields.Histogram[HistogramFields.P75]).Should().BeFalse();
            data["75th_percentile"].Should().Be(11.0);
        }

        [Fact]
        public void Histogram_can_use_custom_field_names()
        {
            // Arrange
            var fieldValues = Enum.GetValues(typeof(HistogramFields));
            const string customKey = "custom";

            // Act
            foreach (HistogramFields field in fieldValues)
            {
                var value = _histogramValue();
                var data = new Dictionary<string, object>();
                var fields = new MetricFields();
                fields.Histogram[field] = customKey;
                value.AddHistogramValues(data, fields.Histogram);

                // Assert
                data.ContainsKey(Fields.Histogram[field]).Should().BeFalse();
                data.ContainsKey(customKey).Should().BeTrue();
            }
        }

        [Fact]
        public void Histogram_default_field_names_should_provide_corresponding_values()
        {
            // Arrange
            var value = _histogramValue();
            var data = new Dictionary<string, object>();

            // Act
            value.AddHistogramValues(data, Fields.Histogram);

            // Assert
            data[Fields.Histogram[HistogramFields.Count]].Should().Be(1L);
            data[Fields.Histogram[HistogramFields.Sum]].Should().Be(1.0);
            data[Fields.Histogram[HistogramFields.LastValue]].Should().Be(2.0);
            data[Fields.Histogram[HistogramFields.UserLastValue]].Should().Be("3");
            data[Fields.Histogram[HistogramFields.Max]].Should().Be(4.0);
            data[Fields.Histogram[HistogramFields.UserMaxValue]].Should().Be("5");
            data[Fields.Histogram[HistogramFields.Mean]].Should().Be(6.0);
            data[Fields.Histogram[HistogramFields.Min]].Should().Be(7.0);
            data[Fields.Histogram[HistogramFields.UserMinValue]].Should().Be("8");
            data[Fields.Histogram[HistogramFields.StdDev]].Should().Be(9.0);
            data[Fields.Histogram[HistogramFields.Median]].Should().Be(10.0);
            data[Fields.Histogram[HistogramFields.P75]].Should().Be(11.0);
            data[Fields.Histogram[HistogramFields.P95]].Should().Be(12.0);
            data[Fields.Histogram[HistogramFields.P98]].Should().Be(13.0);
            data[Fields.Histogram[HistogramFields.P99]].Should().Be(14.0);
            data[Fields.Histogram[HistogramFields.P999]].Should().Be(15.0);
            data[Fields.Histogram[HistogramFields.Samples]].Should().Be(16);
        }

        [Fact]
        public void Histogram_should_ignore_values_where_specified()
        {
            // Arrange
            var fieldValues = Enum.GetValues(typeof(HistogramFields));

            // Act
            foreach (HistogramFields key in fieldValues)
            {
                var value = _histogramValue();
                var data = new Dictionary<string, object>();
                var fields = new MetricFields();
                fields.Histogram.Remove(key);
                value.AddHistogramValues(data, fields.Histogram);

                // Assert
                data.Count.Should().Be(fieldValues.Length - 1);
                data.ContainsKey(Fields.Histogram[key]).Should().BeFalse();
            }
        }

        [Fact]
        public void Histogram_removing_all_fields_shouldnt_throw_or_provide_data()
        {
            // Arrange
            var value = _histogramValue();
            var data = new Dictionary<string, object>();
            var fields = new MetricFields();
            fields.Histogram.Exclude();

            // Act
            Action sut = () => value.AddHistogramValues(data, fields.Histogram);

            // Assert
            sut.Should().NotThrow();
            data.Count.Should().Be(0);
        }
    }
}