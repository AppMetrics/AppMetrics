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
        private readonly Func<MeterValue> _meterValue = () => new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds, new[] { new MeterValue.SetItem("item1", 1.0, new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds)) });

        [Fact]
        public void Meter_can_use_custom_field_names_and_should_provide_corresponding_values()
        {
            // Arrange
            var data = new Dictionary<string, object>();
            var value = _meterValue();
            var fields = new MetricFields();
            fields.Meter.Set(MeterFields.Rate1M, "1_min_rate");
            fields.Meter.Set(MeterFields.RateMean, "mean_rate");

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
            var fieldValues = Enum.GetValues(typeof(MeterFields));
            const string customField = "custom";

            // Act
            foreach (MeterFields field in fieldValues)
            {
                // ignore set item fields
                if (field == MeterFields.SetItem || field == MeterFields.SetItemPercent)
                {
                    continue;
                }

                var data = new Dictionary<string, object>();
                var value = _meterValue();
                var fields = new MetricFields();
                fields.Meter[field] = customField;
                value.AddMeterValues(data, fields.Meter);

                // Assert
                data.ContainsKey(Fields.Meter[field]).Should().BeFalse($"{field} has been removed");
                data.ContainsKey(customField).Should().BeTrue($"{field} has been replaced with {customField}");
            }
        }

        [Fact]
        public void Meter_set_item_percent_can_use_custom_field_name()
        {
            // Arrange
            var data = new Dictionary<string, object>();
            const string customField = "custom";

            // Act
            var value = _meterSetItemsValue();
            var fields = new MetricFields();
            fields.Meter[MeterFields.SetItemPercent] = customField;
            value.AddMeterSetItemValues(data, fields.Meter);

            // Assert
            data.ContainsKey(Fields.Meter[MeterFields.SetItemPercent]).Should().BeFalse($"{MeterFields.SetItemPercent} has been removed");
            data.ContainsKey(customField).Should().BeTrue($"{MeterFields.SetItemPercent} has been replaced with {customField}");
        }

        [Fact]
        public void Can_exclude_meter_set_items()
        {
            // Arrange
            var data = new Dictionary<string, object>();

            // Act
            var value = _meterSetItemsValue();
            var fields = new MetricFields();
            fields.Meter.Exclude(MeterFields.SetItem);
            value.AddMeterSetItemValues(data, fields.Meter);

            // Assert
            data.Count.Should().Be(0, "SetItem field was ignored");
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
            var setItemFields = new[] { MeterFields.SetItem, MeterFields.SetItemPercent };
            var meterFields = Enum.GetValues(typeof(MeterFields));

            // Act
            foreach (MeterFields field in meterFields)
            {
                if (field == MeterFields.SetItem || field == MeterFields.SetItemPercent)
                {
                    continue;
                }

                var data = new Dictionary<string, object>();
                var value = _meterValue();
                var fields = new MetricFields();
                fields.Meter.Remove(field);
                value.AddMeterValues(data, fields.Meter);

                // Assert
                data.Count.Should().Be(meterFields.Length - setItemFields.Length - 1);
                data.ContainsKey(Fields.Meter[field]).Should().BeFalse();
            }
        }

        [Fact]
        public void Meter_set_items_should_ignore_values_where_specified()
        {
            // Arrange
            var data = new Dictionary<string, object>();
            var value = _meterSetItemsValue();
            var fields = new MetricFields();
            fields.Meter.Remove(MeterFields.SetItem);

            // Act
            value.AddMeterSetItemValues(data, fields.Meter);

            // Assert
            data.Count.Should().Be(0, "SetItems field was removed");
        }

        [Fact]
        public void Meter_set_item_percent_can_be_ignored()
        {
            // Arrange
            var data = new Dictionary<string, object>();

            // Act
            var value = _meterSetItemsValue();
            var fields = new MetricFields();
            fields.Meter.Exclude(MeterFields.SetItemPercent);
            value.AddMeterSetItemValues(data, fields.Meter);

            // Assert
            data.ContainsKey(Fields.Meter[MeterFields.SetItemPercent]).Should().BeFalse($"{MeterFields.SetItemPercent} has been removed");
        }

        [Fact]
        public void Meter_removing_all_fields_shouldnt_throw_or_provide_data()
        {
            // Arrange
            var value = _meterValue();
            var data = new Dictionary<string, object>();
            var fields = new MetricFields();
            fields.Meter.Exclude();

            // Act
            Action sut = () => value.AddMeterValues(data, fields.Meter);

            // Assert
            sut.Should().NotThrow();
            data.Count.Should().Be(0);
        }
    }
}