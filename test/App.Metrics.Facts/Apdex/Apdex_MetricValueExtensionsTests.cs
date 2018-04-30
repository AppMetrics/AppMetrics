// <copyright file="Apdex_MetricValueExtensionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Apdex;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Apdex
{
    // ReSharper disable InconsistentNaming
    public class Apdex_MetricValueExtensionsTests
        // ReSharper restore InconsistentNaming
    {
        private static readonly GeneratedMetricNameMapping DataKeys = new GeneratedMetricNameMapping();
        private readonly Func<ApdexValue> _apdexValue = () => new ApdexValue(1, 2, 3, 4, 5);

        [Fact]
        public void Apdex_can_use_custom_data_keys_and_should_provide_corresponding_values()
        {
            // Arrange
            var keys = Enum.GetValues(typeof(ApdexValueDataKeys));
            const string customKey = "custom";

            // Act
            foreach (ApdexValueDataKeys key in keys)
            {
                var value = _apdexValue();
                var data = new Dictionary<string, object>();
                var dataKeys = new GeneratedMetricNameMapping();
                dataKeys.Apdex[key] = customKey;
                value.AddApdexValues(data, dataKeys.Apdex);

                // Assert
                data.ContainsKey(DataKeys.Apdex[key]).Should().BeFalse();
                data.ContainsKey(customKey).Should().BeTrue();
            }
        }

        [Fact]
        public void Apdex_should_ignore_values_where_specified()
        {
            // Arrange
            var keys = Enum.GetValues(typeof(ApdexValueDataKeys));

            // Act
            foreach (ApdexValueDataKeys key in keys)
            {
                var value = _apdexValue();
                var data = new Dictionary<string, object>();
                var dataKeys = new GeneratedMetricNameMapping();
                dataKeys.Apdex.Remove(key);
                value.AddApdexValues(data, dataKeys.Apdex);

                // Assert
                data.Count.Should().Be(keys.Length - 1);
                data.ContainsKey(DataKeys.Apdex[key]).Should().BeFalse();
            }
        }

        [Fact]
        public void Apdex_default_data_keys_should_provide_corresponding_values()
        {
            // Arrange
            var value = _apdexValue();
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

        [Fact]
        public void Apdex_removing_all_keys_shouldnt_throw_or_provide_data()
        {
            // Arrange
            var value = _apdexValue();
            var data = new Dictionary<string, object>();
            var dataKeys = new GeneratedMetricNameMapping();
            dataKeys.ExcludeApdexValues();

            // Act
            Action sut = () => value.AddApdexValues(data, dataKeys.Apdex);

            // Assert
            sut.Should().NotThrow();
            data.Count.Should().Be(0);
        }
    }
}