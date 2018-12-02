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
        private static readonly MetricFields Fields = new MetricFields();
        private readonly Func<ApdexValue> _apdexValue = () => new ApdexValue(1, 2, 3, 4, 5);

        [Fact]
        public void Apdex_can_use_custom_field_names_and_should_provide_corresponding_values()
        {
            // Arrange
            var keys = Enum.GetValues(typeof(ApdexFields));
            const string customKey = "custom";

            // Act
            foreach (ApdexFields key in keys)
            {
                var value = _apdexValue();
                var data = new Dictionary<string, object>();
                var fields = new MetricFields();
                fields.Apdex[key] = customKey;
                value.AddApdexValues(data, fields.Apdex);

                // Assert
                data.ContainsKey(Fields.Apdex[key]).Should().BeFalse();
                data.ContainsKey(customKey).Should().BeTrue();
            }
        }

        [Fact]
        public void Apdex_should_ignore_values_where_specified()
        {
            // Arrange
            var fieldValues = Enum.GetValues(typeof(ApdexFields));

            // Act
            foreach (ApdexFields key in fieldValues)
            {
                var value = _apdexValue();
                var data = new Dictionary<string, object>();
                var fields = new MetricFields();
                fields.Apdex.Remove(key);
                value.AddApdexValues(data, fields.Apdex);

                // Assert
                data.Count.Should().Be(fieldValues.Length - 1);
                data.ContainsKey(Fields.Apdex[key]).Should().BeFalse();
            }
        }

        [Fact]
        public void Apdex_default_field_names_should_provide_corresponding_values()
        {
            // Arrange
            var value = _apdexValue();
            var data = new Dictionary<string, object>();

            // Act
            value.AddApdexValues(data, Fields.Apdex);

            // Assert
            data[Fields.Apdex[ApdexFields.Score]].Should().Be(1.0);
            data[Fields.Apdex[ApdexFields.Satisfied]].Should().Be(2);
            data[Fields.Apdex[ApdexFields.Tolerating]].Should().Be(3);
            data[Fields.Apdex[ApdexFields.Frustrating]].Should().Be(4);
            data[Fields.Apdex[ApdexFields.Samples]].Should().Be(5);
        }

        [Fact]
        public void Apdex_removing_all_fields_shouldnt_throw_or_provide_data()
        {
            // Arrange
            var value = _apdexValue();
            var data = new Dictionary<string, object>();
            var fields = new MetricFields();
            fields.Apdex.Exclude();

            // Act
            Action sut = () => value.AddApdexValues(data, fields.Apdex);

            // Assert
            sut.Should().NotThrow();
            data.Count.Should().Be(0);
        }
    }
}