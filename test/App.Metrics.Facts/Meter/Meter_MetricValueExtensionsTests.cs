// <copyright file="Meter_MetricValueExtensionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Meter;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Meter
{
    // ReSharper disable InconsistentNaming
    public class Meter_MetricValueExtensionsTests
        // ReSharper restore InconsistentNaming
    {
        private static readonly MetricFields Fields = new MetricFields();

        private readonly Func<MeterValue.SetItem> _meterSetItemsValue = () => new MeterValue.SetItem("item1", 1.0, new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds));
        private readonly Func<MeterValue> _meterValue = () => new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds);

        [Fact]
        public void Meter_can_use_custom_field_names_and_should_provide_corresponding_values()
        {
            // Arrange
            var data = new Dictionary<string, object>();
            var value = _meterValue();
            var fields = new MetricFields(
                meter: new Dictionary<MeterFields, string>
                       {
                           { MeterFields.Rate1M, "1_min_rate" },
                           { MeterFields.RateMean, "mean_rate" }
                       });

            // Act
            value.AddMeterValues(data, fields.Meter);

            // Assert
            data.ContainsKey(Fields.Meter[MeterFields.RateMean]).Should().BeFalse();
            data["mean_rate"].Should().Be(2.0);
            data.ContainsKey(Fields.Meter[MeterFields.Rate1M]).Should().BeFalse();
            data["1_min_rate"].Should().Be(3.0);
        }

        [Fact]
        public void Meter_can_use_custom_field_names()
        {
            // Arrange
            var keys = Enum.GetValues(typeof(MeterFields));
            const string customKey = "custom";

            // Act
            foreach (MeterFields key in keys)
            {
                // TODO: Refactoring AppMetrics/AppMetrics/#251
                if (key == MeterFields.MetricSetItemSuffix || key == MeterFields.SetItemPercent)
                {
                    continue;
                }

                var data = new Dictionary<string, object>();
                var value = _meterValue();
                var fields = new MetricFields();
                fields.Meter[key] = customKey;
                value.AddMeterValues(data, fields.Meter);

                // Assert
                data.ContainsKey(Fields.Meter[key]).Should().BeFalse($"{key} has been removed");
                data.ContainsKey(customKey).Should().BeTrue($"{key} has been replaced with {customKey}");
            }
        }

        [Fact]
        public void Meter_set_item_can_use_custom_field_names()
        {
            // Arrange
            var data = new Dictionary<string, object>();
            var fieldValues = Enum.GetValues(typeof(MeterFields));
            const string customKey = "custom";

            // Act
            foreach (MeterFields field in fieldValues)
            {
                // TODO: Refactoring AppMetrics/AppMetrics/#251
                if (field != MeterFields.MetricSetItemSuffix || field != MeterFields.SetItemPercent)
                {
                    continue;
                }

                var value = _meterSetItemsValue();
                var fields = new MetricFields();
                fields.Meter[field] = customKey;
                value.AddMeterSetItemValues(data, fields.Meter);

                // Assert
                data.ContainsKey(Fields.Meter[field]).Should().BeFalse($"{field} has been removed");
                data.ContainsKey(customKey).Should().BeTrue($"{field} has been replaced with {customKey}");
            }
        }

        [Fact]
        public void Meter_default_field_names_should_provide_corresponding_values()
        {
            // Arrange
            var data = new Dictionary<string, object>();
            var value = _meterValue();

            // Act
            value.AddMeterValues(data, Fields.Meter);

            // Assert
            data[Fields.Meter[MeterFields.Count]].Should().Be(1L);
            data[Fields.Meter[MeterFields.RateMean]].Should().Be(2.0);
            data[Fields.Meter[MeterFields.Rate1M]].Should().Be(3.0);
            data[Fields.Meter[MeterFields.Rate5M]].Should().Be(4.0);
            data[Fields.Meter[MeterFields.Rate15M]].Should().Be(5.0);
        }

        [Fact]
        public void Meter_should_ignore_values_where_specified()
        {
            // Arrange
            var data = new Dictionary<string, object>();
            var keys = Enum.GetValues(typeof(MeterFields));
            var meterKeys = new List<MeterFields>();
            foreach (MeterFields key in keys)
            {
                // TODO: Refactoring AppMetrics/AppMetrics/#251
                if (key != MeterFields.MetricSetItemSuffix || key != MeterFields.SetItemPercent)
                {
                    continue;
                }

                meterKeys.Add(key);
            }

            // Act
            foreach (var key in meterKeys)
            {
                var value = _meterValue();
                var fields = new MetricFields();
                fields.Meter.Remove(key);
                value.AddMeterValues(data, fields.Meter);

                // Assert
                data.Count.Should().Be(meterKeys.Count - 1);
                data.ContainsKey(Fields.Meter[key]).Should().BeFalse();
            }
        }

        [Fact]
        public void Meter_set_items_should_ignore_values_where_specified()
        {
            // Arrange
            var setItemKeys = new List<MeterFields> { MeterFields.MetricSetItemSuffix, MeterFields.SetItemPercent };

            // Act
            foreach (var key in setItemKeys)
            {
                var data = new Dictionary<string, object>();
                var value = _meterSetItemsValue();
                var fields = new MetricFields();
                fields.Meter.Remove(key);
                value.AddMeterSetItemValues(data, fields.Meter);

                // Assert
                // TODO: Refactoring AppMetrics/AppMetrics/#251, between 5 and 6 because of set items
                data.Count.Should().BeInRange(5, 6);
                data.ContainsKey(Fields.Meter[key]).Should().BeFalse();
            }
        }

        [Fact]
        public void Meter_removing_all_fields_shouldnt_throw_or_provide_data()
        {
            // Arrange
            var value = _meterValue();
            var data = new Dictionary<string, object>();
            var fields = new MetricFields();
            fields.ExcludeMeterValues();

            // Act
            Action sut = () => value.AddMeterValues(data, fields.Meter);

            // Assert
            sut.Should().NotThrow();
            data.Count.Should().Be(0);
        }
    }
}