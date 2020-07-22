// <copyright file="VolatileTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

#if !NET45
using System.Runtime.InteropServices;
#endif
using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class VolatileTests
    {
        [Fact]
        public void Can_get_and_set_value()
        {
            var value = new VolatileDouble(1.5);

            value.GetValue().Should().Be(1.5);
            value.SetValue(2.3);
            value.GetValue().Should().Be(2.3);
        }

#if !NET45
        [Fact]
        public void Has_correct_size()
        {
            VolatileDouble.SizeInBytes.Should().Be(Marshal.SizeOf<VolatileDouble>());
        }
#endif

        [Fact]
        public void Can_get_and_set_value_non_volatile()
        {
            var value = new VolatileDouble(1.5);

            value.NonVolatileGetValue().Should().Be(1.5);
            value.NonVolatileSetValue(2.3);
            value.NonVolatileGetValue().Should().Be(2.3);
        }
    }
}