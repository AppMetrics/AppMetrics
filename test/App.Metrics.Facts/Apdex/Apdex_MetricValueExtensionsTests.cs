// <copyright file="Apdex_MetricValueExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Apdex;
using App.Metrics.Formatters;
using App.Metrics.Internal;
using App.Metrics.Serialization;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Apdex
{
    // ReSharper disable InconsistentNaming
    public class Apdex_MetricValueExtensionsTests
        // ReSharper restore InconsistentNaming
    {
        private static readonly GeneratedMetricNameMapping DataKeys = new GeneratedMetricNameMapping();

        [Fact]
        public void Apdex_can_use_custom_data_keys_and_should_provide_corresponding_values()
        {
            // Arrange
            var value = new ApdexValue(1, 2, 3, 4, 5);
            var data = new Dictionary<string, object>();
            var dataKeys = new GeneratedMetricNameMapping(
                apdex: new Dictionary<ApdexValueDataKeys, string>
                       {
                           { ApdexValueDataKeys.Samples, "size_of_sample" }
                       });

            // Act
            value.AddApdexValues(data, dataKeys.Apdex);

            // Assert
            data.ContainsKey(DataKeys.Apdex[ApdexValueDataKeys.Samples]).Should().BeFalse();
            data["size_of_sample"].Should().Be(5);
        }

        [Fact]
        public void Apdex_default_data_keys_should_provide_corresponding_values()
        {
            // Arrange
            var value = new ApdexValue(1, 2, 3, 4, 5);
            var data = new Dictionary<string, object>();

            // Act
            value.AddApdexValues(data, DataKeys.Apdex);

            // Assert
            data[DataKeys.Apdex[ApdexValueDataKeys.Score]].Should().Be(1.0);
            data[DataKeys.Apdex[ApdexValueDataKeys.Satisfied]].Should().Be(2);
            data[DataKeys.Apdex[ApdexValueDataKeys.Tolerating]].Should().Be(3);
            data[DataKeys.Apdex[ApdexValueDataKeys.Frustrating]].Should().Be(4);
            data[DataKeys.Apdex[ApdexValueDataKeys.Samples]].Should().Be(5);
        }
    }
}