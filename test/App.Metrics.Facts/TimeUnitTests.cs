// <copyright file="TimeUnitTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts
{
    public class TimeUnitTests
    {
        [Fact]
        public void Can_convert_between_units()
        {
            TimeUnit.Nanoseconds.ToNanoseconds(10L).Should().Be(10L);
            TimeUnit.Nanoseconds.ToMicroseconds(10L * 1000L).Should().Be(10L);
            TimeUnit.Nanoseconds.ToMilliseconds(10L * 1000L * 1000L).Should().Be(10L);
            TimeUnit.Nanoseconds.ToSeconds(10L * 1000L * 1000L * 1000L).Should().Be(10L);
            TimeUnit.Nanoseconds.ToMinutes(10L * 1000L * 1000L * 1000L * 60L).Should().Be(10L);
            TimeUnit.Nanoseconds.ToHours(10L * 1000L * 1000L * 1000L * 60L * 60L).Should().Be(10L);
            TimeUnit.Nanoseconds.ToDays(10L * 1000L * 1000L * 1000L * 60L * 60L * 24L).Should().Be(10L);

            TimeUnit.Microseconds.ToNanoseconds(10L).Should().Be(10L * 1000L);
            TimeUnit.Microseconds.ToMicroseconds(10L).Should().Be(10L);
            TimeUnit.Microseconds.ToMilliseconds(10L * 1000L).Should().Be(10L);
            TimeUnit.Microseconds.ToSeconds(10L * 1000L * 1000L).Should().Be(10L);
            TimeUnit.Microseconds.ToMinutes(10L * 1000L * 1000L * 60L).Should().Be(10L);
            TimeUnit.Microseconds.ToHours(10L * 1000L * 1000L * 60L * 60L).Should().Be(10L);
            TimeUnit.Microseconds.ToDays(10L * 1000L * 1000L * 60L * 60L * 24L).Should().Be(10L);

            TimeUnit.Milliseconds.ToNanoseconds(10L).Should().Be(10L * 1000L * 1000L);
            TimeUnit.Milliseconds.ToMicroseconds(10L).Should().Be(10L * 1000L);
            TimeUnit.Milliseconds.ToMilliseconds(10L).Should().Be(10L);
            TimeUnit.Milliseconds.ToSeconds(10L * 1000L).Should().Be(10L);
            TimeUnit.Milliseconds.ToMinutes(10L * 1000L * 60L).Should().Be(10L);
            TimeUnit.Milliseconds.ToHours(10L * 1000L * 60L * 60L).Should().Be(10L);
            TimeUnit.Milliseconds.ToDays(10L * 1000L * 60L * 60L * 24L).Should().Be(10L);

            TimeUnit.Seconds.ToNanoseconds(10L).Should().Be(10L * 1000L * 1000L * 1000L);
            TimeUnit.Seconds.ToMicroseconds(10L).Should().Be(10L * 1000L * 1000L);
            TimeUnit.Seconds.ToMilliseconds(10L).Should().Be(10L * 1000L);
            TimeUnit.Seconds.ToSeconds(10L).Should().Be(10L);
            TimeUnit.Seconds.ToMinutes(10L * 60L).Should().Be(10L);
            TimeUnit.Seconds.ToHours(10L * 60L * 60L).Should().Be(10L);
            TimeUnit.Seconds.ToDays(10L * 60L * 60L * 24L).Should().Be(10L);

            TimeUnit.Minutes.ToNanoseconds(10L).Should().Be(10L * 1000L * 1000L * 1000L * 60L);
            TimeUnit.Minutes.ToMicroseconds(10L).Should().Be(10L * 1000L * 1000L * 60L);
            TimeUnit.Minutes.ToMilliseconds(10L).Should().Be(10L * 1000L * 60L);
            TimeUnit.Minutes.ToSeconds(10L).Should().Be(10L * 60L);
            TimeUnit.Minutes.ToMinutes(10L).Should().Be(10L);
            TimeUnit.Minutes.ToHours(10L * 60L).Should().Be(10L);
            TimeUnit.Minutes.ToDays(10L * 60L * 24L).Should().Be(10L);

            TimeUnit.Hours.ToNanoseconds(10L).Should().Be(10L * 1000L * 1000L * 1000L * 60L * 60L);
            TimeUnit.Hours.ToMicroseconds(10L).Should().Be(10L * 1000L * 1000L * 60L * 60L);
            TimeUnit.Hours.ToMilliseconds(10L).Should().Be(10L * 1000L * 60L * 60L);
            TimeUnit.Hours.ToSeconds(10L).Should().Be(10L * 60L * 60L);
            TimeUnit.Hours.ToMinutes(10L).Should().Be(10L * 60L);
            TimeUnit.Hours.ToHours(10L).Should().Be(10L);
            TimeUnit.Hours.ToDays(10L * 24L).Should().Be(10L);

            TimeUnit.Days.ToNanoseconds(10L).Should().Be(10L * 1000L * 1000L * 1000L * 60L * 60L * 24L);
            TimeUnit.Days.ToMicroseconds(10L).Should().Be(10L * 1000L * 1000L * 60L * 60L * 24L);
            TimeUnit.Days.ToMilliseconds(10L).Should().Be(10L * 1000L * 60L * 60L * 24L);
            TimeUnit.Days.ToSeconds(10L).Should().Be(10L * 60L * 60L * 24L);
            TimeUnit.Days.ToMinutes(10L).Should().Be(10L * 60L * 24L);
            TimeUnit.Days.ToHours(10L).Should().Be(10L * 24L);
            TimeUnit.Days.ToDays(10L).Should().Be(10L);
        }

        [Fact]
        public void Converts_to_zero_on_fractional_unit()
        {
            TimeUnit.Seconds.ToMinutes(30L).Should().Be(0);
        }

        [Fact]
        public void Has_correct_scaling_factor()
        {
            TimeUnit.Nanoseconds.ScalingFactorFor(TimeUnit.Nanoseconds).Should().Be(1.0);
            TimeUnit.Nanoseconds.ScalingFactorFor(TimeUnit.Microseconds).Should().Be(1.0 * 0.001);
            TimeUnit.Nanoseconds.ScalingFactorFor(TimeUnit.Milliseconds).Should().Be(1.0 * 0.001 * 0.001);
            TimeUnit.Nanoseconds.ScalingFactorFor(TimeUnit.Seconds).Should().Be(1.0 * 0.001 * 0.001 * 0.001);
            TimeUnit.Nanoseconds.ScalingFactorFor(TimeUnit.Minutes).Should().Be(1.0 * 0.001 * 0.001 * 0.001 * 1 / 60.0);
            TimeUnit.Nanoseconds.ScalingFactorFor(TimeUnit.Hours).Should().Be(1.0 * 0.001 * 0.001 * 0.001 * 1 / 60.0 * 1 / 60.0);
            TimeUnit.Nanoseconds.ScalingFactorFor(TimeUnit.Days).Should().
                     BeApproximately(1.0 * 0.001 * 0.001 * 0.001 * 1 / 60.0 * 1 / 60.0 * 1 / 24.0, 1.0E-20);

            TimeUnit.Microseconds.ScalingFactorFor(TimeUnit.Nanoseconds).Should().Be(1.0 * 1000);
            TimeUnit.Microseconds.ScalingFactorFor(TimeUnit.Microseconds).Should().Be(1);
            TimeUnit.Microseconds.ScalingFactorFor(TimeUnit.Milliseconds).Should().Be(1.0 * 0.001);
            TimeUnit.Microseconds.ScalingFactorFor(TimeUnit.Seconds).Should().Be(1.0 * 0.001 * 0.001);
            TimeUnit.Microseconds.ScalingFactorFor(TimeUnit.Minutes).Should().Be(1.0 * 0.001 * 0.001 * 1 / 60.0);
            TimeUnit.Microseconds.ScalingFactorFor(TimeUnit.Hours).Should().Be(1.0 * 0.001 * 0.001 * 1 / 60.0 * 1 / 60.0);
            TimeUnit.Microseconds.ScalingFactorFor(TimeUnit.Days).Should().Be(1.0 * 0.001 * 0.001 * 1 / 60.0 * 1 / 60.0 * 1 / 24.0);

            TimeUnit.Milliseconds.ScalingFactorFor(TimeUnit.Nanoseconds).Should().Be(1.0 * 1000 * 1000);
            TimeUnit.Milliseconds.ScalingFactorFor(TimeUnit.Microseconds).Should().Be(1.0 * 1000);
            TimeUnit.Milliseconds.ScalingFactorFor(TimeUnit.Milliseconds).Should().Be(1);
            TimeUnit.Milliseconds.ScalingFactorFor(TimeUnit.Seconds).Should().Be(1.0 * 0.001);
            TimeUnit.Milliseconds.ScalingFactorFor(TimeUnit.Minutes).Should().Be(1.0 * 0.001 * 1 / 60.0);
            TimeUnit.Milliseconds.ScalingFactorFor(TimeUnit.Hours).Should().BeApproximately(1.0 * 0.001 * 1 / 60.0 * 1 / 60.0, 1.0E-20);
            TimeUnit.Milliseconds.ScalingFactorFor(TimeUnit.Days).Should().BeApproximately(1.0 * 0.001 * 1 / 60.0 * 1 / 60.0 * 1 / 24.0, 1.0E-20);

            TimeUnit.Seconds.ScalingFactorFor(TimeUnit.Nanoseconds).Should().Be(1.0 * 1000 * 1000 * 1000);
            TimeUnit.Seconds.ScalingFactorFor(TimeUnit.Microseconds).Should().Be(1.0 * 1000 * 1000);
            TimeUnit.Seconds.ScalingFactorFor(TimeUnit.Milliseconds).Should().Be(1.0 * 1000);
            TimeUnit.Seconds.ScalingFactorFor(TimeUnit.Seconds).Should().Be(1.0);
            TimeUnit.Seconds.ScalingFactorFor(TimeUnit.Minutes).Should().Be(1.0 * 1 / 60);
            TimeUnit.Seconds.ScalingFactorFor(TimeUnit.Hours).Should().Be(1.0 * 1 / 60 * 1 / 60);
            TimeUnit.Seconds.ScalingFactorFor(TimeUnit.Days).Should().Be(1.0 * 1 / 60 * 1 / 60 * 1 / 24);

            TimeUnit.Minutes.ScalingFactorFor(TimeUnit.Nanoseconds).Should().Be(1.0 * 1000 * 1000 * 1000 * 60);
            TimeUnit.Minutes.ScalingFactorFor(TimeUnit.Microseconds).Should().Be(1.0 * 1000 * 1000 * 60);
            TimeUnit.Minutes.ScalingFactorFor(TimeUnit.Milliseconds).Should().Be(1.0 * 1000 * 60);
            TimeUnit.Minutes.ScalingFactorFor(TimeUnit.Seconds).Should().Be(1.0 * 60);
            TimeUnit.Minutes.ScalingFactorFor(TimeUnit.Minutes).Should().Be(1.0);
            TimeUnit.Minutes.ScalingFactorFor(TimeUnit.Hours).Should().Be(1.0 / 60);
            TimeUnit.Minutes.ScalingFactorFor(TimeUnit.Days).Should().Be(1.0 / 60 / 24);

            TimeUnit.Hours.ScalingFactorFor(TimeUnit.Nanoseconds).Should().Be(1.0 * 1000 * 1000 * 1000 * 60 * 60);
            TimeUnit.Hours.ScalingFactorFor(TimeUnit.Microseconds).Should().Be(1.0 * 1000 * 1000 * 60 * 60);
            TimeUnit.Hours.ScalingFactorFor(TimeUnit.Milliseconds).Should().Be(1.0 * 1000 * 60 * 60);
            TimeUnit.Hours.ScalingFactorFor(TimeUnit.Seconds).Should().Be(1.0 * 60 * 60);
            TimeUnit.Hours.ScalingFactorFor(TimeUnit.Minutes).Should().Be(1.0 * 60);
            TimeUnit.Hours.ScalingFactorFor(TimeUnit.Hours).Should().Be(1);
            TimeUnit.Hours.ScalingFactorFor(TimeUnit.Days).Should().Be(1.0 / 24);

            TimeUnit.Days.ScalingFactorFor(TimeUnit.Nanoseconds).Should().Be(1.0 * 1000 * 1000 * 1000 * 60 * 60 * 24);
            TimeUnit.Days.ScalingFactorFor(TimeUnit.Microseconds).Should().Be(1.0 * 1000 * 1000 * 60 * 60 * 24);
            TimeUnit.Days.ScalingFactorFor(TimeUnit.Milliseconds).Should().Be(1.0 * 1000 * 60 * 60 * 24);
            TimeUnit.Days.ScalingFactorFor(TimeUnit.Seconds).Should().Be(1.0 * 60 * 60 * 24);
            TimeUnit.Days.ScalingFactorFor(TimeUnit.Minutes).Should().Be(1.0 * 60 * 24);
            TimeUnit.Days.ScalingFactorFor(TimeUnit.Hours).Should().Be(1.0 * 24);
            TimeUnit.Days.ScalingFactorFor(TimeUnit.Days).Should().Be(1.0);
        }
    }
}