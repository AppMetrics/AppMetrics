// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Meter;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Meter
{
    public class MeterValueSetItemEqualityTests
    {
        [Fact]
        public void equality_with_equals_false_when_not_same_by_percent()
        {
            var meterValue = new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds);

            var value = new MeterValue.SetItem("item", 0.5, meterValue);
            var other = new MeterValue.SetItem("item", 0.6, meterValue);

            value.Equals(other).Should().Be(false);
        }

        [Fact]
        public void equality_with_equals_false_when_not_same_by_value()
        {
            var meterValue = new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds);

            var value = new MeterValue.SetItem("item", 0.5, meterValue);
            var other = new MeterValue.SetItem("item2", 0.5, meterValue);

            value.Equals(other).Should().Be(false);
        }

        [Fact]
        public void equality_with_equals_false_when_same_object()
        {
            var meterValue = new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds);

            var value = new MeterValue.SetItem("item", 0.5, meterValue);
            object other = value;

            value.Equals(other).Should().Be(true);
        }

        [Fact]
        public void equality_with_equals_operator()
        {
            var meterValue = new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds);
            
            var value = new MeterValue.SetItem("item", 0.5, meterValue);
            var other = new MeterValue.SetItem("item", 0.5, meterValue);

            (value == other).Should().Be(true);
        }

        [Fact]
        public void equality_with_equals_true_when_same()
        {
            var meterValue = new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds);

            var value = new MeterValue.SetItem("item", 0.5, meterValue);
            var other = new MeterValue.SetItem("item", 0.5, meterValue);

            value.Equals(other).Should().Be(true);
        }

        [Fact]
        public void equality_with_not_equals_operator()
        {
            var meterValue = new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds);

            var value = new MeterValue.SetItem("item", 0.5, meterValue);
            var other = new MeterValue.SetItem("item", 0.5, meterValue);

            (value != other).Should().Be(false);
        }
    }
}