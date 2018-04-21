// <copyright file="Meter_MetricValueExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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
        private static readonly GeneratedMetricNameMapping DataKeys = new GeneratedMetricNameMapping();

        private readonly Func<MeterValue.SetItem> _meterSetItemsValue = () => new MeterValue.SetItem("item1", 1.0, new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds));
        private readonly Func<MeterValue> _meterValue = () => new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds);

        [Fact]
        public void Meter_can_use_custom_data_keys_and_should_provide_corresponding_values()
        {
            // Arrange
            var value = _meterValue();
            var dataKeys = new GeneratedMetricNameMapping(
                meter: new Dictionary<MeterValueDataKeys, string>
                       {
                           { MeterValueDataKeys.Rate1M, "1_min_rate" },
                           { MeterValueDataKeys.RateMean, "mean_rate" }
                       });

            // Act
            value.AddMeterValues(out var data, dataKeys.Meter);

            // Assert
            data.ContainsKey(DataKeys.Meter[MeterValueDataKeys.RateMean]).Should().BeFalse();
            data["mean_rate"].Should().Be(2.0);
            data.ContainsKey(DataKeys.Meter[MeterValueDataKeys.Rate1M]).Should().BeFalse();
            data["1_min_rate"].Should().Be(3.0);
        }

        [Fact]
        public void Meter_can_use_custom_data_keys()
        {
            // Arrange
            var keys = Enum.GetValues(typeof(MeterValueDataKeys));
            const string customKey = "custom";

            // Act
            foreach (MeterValueDataKeys key in keys)
            {
                // TODO: Refactoring AppMetrics/AppMetrics/#251
                if (key == MeterValueDataKeys.MetricSetItemSuffix || key == MeterValueDataKeys.SetItemPercent)
                {
                    continue;
                }

                var value = _meterValue();
                var dataKeys = new GeneratedMetricNameMapping();
                dataKeys.Meter[key] = customKey;
                value.AddMeterValues(out var data, dataKeys.Meter);

                // Assert
                data.ContainsKey(DataKeys.Meter[key]).Should().BeFalse($"{key} has been removed");
                data.ContainsKey(customKey).Should().BeTrue($"{key} has been replaced with {customKey}");
            }
        }

        [Fact]
        public void Meter_set_item_can_use_custom_data_keys()
        {
            // Arrange
            var keys = Enum.GetValues(typeof(MeterValueDataKeys));
            const string customKey = "custom";

            // Act
            foreach (MeterValueDataKeys key in keys)
            {
                // TODO: Refactoring AppMetrics/AppMetrics/#251
                if (key != MeterValueDataKeys.MetricSetItemSuffix || key != MeterValueDataKeys.SetItemPercent)
                {
                    continue;
                }

                var value = _meterSetItemsValue();
                var dataKeys = new GeneratedMetricNameMapping();
                dataKeys.Meter[key] = customKey;
                value.AddMeterSetItemValues(out var data, dataKeys.Meter);

                // Assert
                data.ContainsKey(DataKeys.Meter[key]).Should().BeFalse($"{key} has been removed");
                data.ContainsKey(customKey).Should().BeTrue($"{key} has been replaced with {customKey}");
            }
        }

        [Fact]
        public void Meter_default_data_keys_should_provide_corresponding_values()
        {
            // Arrange
            var value = _meterValue();

            // Act
            value.AddMeterValues(out var data, DataKeys.Meter);

            // Assert
            data[DataKeys.Meter[MeterValueDataKeys.Count]].Should().Be(1L);
            data[DataKeys.Meter[MeterValueDataKeys.RateMean]].Should().Be(2.0);
            data[DataKeys.Meter[MeterValueDataKeys.Rate1M]].Should().Be(3.0);
            data[DataKeys.Meter[MeterValueDataKeys.Rate5M]].Should().Be(4.0);
            data[DataKeys.Meter[MeterValueDataKeys.Rate15M]].Should().Be(5.0);
        }

        [Fact]
        public void Meter_should_ignore_values_where_specified()
        {
            // Arrange
            var keys = Enum.GetValues(typeof(MeterValueDataKeys));
            var meterKeys = new List<MeterValueDataKeys>();
            foreach (MeterValueDataKeys key in keys)
            {
                // TODO: Refactoring AppMetrics/AppMetrics/#251
                if (key != MeterValueDataKeys.MetricSetItemSuffix || key != MeterValueDataKeys.SetItemPercent)
                {
                    continue;
                }

                meterKeys.Add(key);
            }

            // Act
            foreach (var key in meterKeys)
            {
                var value = _meterValue();
                var dataKeys = new GeneratedMetricNameMapping();
                dataKeys.Meter.Remove(key);
                value.AddMeterValues(out var data, dataKeys.Meter);

                // Assert
                data.Count.Should().Be(meterKeys.Count - 1);
                data.ContainsKey(DataKeys.Meter[key]).Should().BeFalse();
            }
        }

        [Fact]
        public void Meter_set_items_should_ignore_values_where_specified()
        {
            // Arrange
            var setItemKeys = new List<MeterValueDataKeys> { MeterValueDataKeys.MetricSetItemSuffix, MeterValueDataKeys.SetItemPercent };

            // Act
            foreach (var key in setItemKeys)
            {
                var value = _meterSetItemsValue();
                var dataKeys = new GeneratedMetricNameMapping();
                dataKeys.Meter.Remove(key);
                value.AddMeterSetItemValues(out var data, dataKeys.Meter);

                // Assert
                // TODO: Refactoring AppMetrics/AppMetrics/#251, between 5 and 6 because of set items
                data.Count.Should().BeInRange(5, 6);
                data.ContainsKey(DataKeys.Meter[key]).Should().BeFalse();
            }
        }
    }
}