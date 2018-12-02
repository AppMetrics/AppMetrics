// <copyright file="MetricsClockBuilderTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.FactsCommon;
using App.Metrics.Infrastructure;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Builders
{
    public class MetricsClockBuilderTests
    {
        [Fact]
        public void Can_set_clock__to_stopwatch_clock_should_the_last_selected_clock()
        {
            // Arrange
            var builder = new MetricsBuilder().TimeWith.Clock<SystemClock>();

            // Act
            builder.TimeWith.StopwatchClock();
            var metrics = builder.Build();

            // Assert
            metrics.Clock.Should().BeOfType<StopwatchClock>();
        }

        [Fact]
        public void Can_set_clock_to_custom_implementation()
        {
            // Arrange
            var builder = new MetricsBuilder().TimeWith.Clock<TestClock>();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Clock.Should().BeOfType<TestClock>();
        }

        [Fact]
        public void Can_set_clock_to_system_clock()
        {
            // Arrange
            var builder = new MetricsBuilder().TimeWith.SystemClock();

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Clock.Should().BeOfType<SystemClock>();
        }

        [Fact]
        public void Can_set_clock_using_instantiation()
        {
            // Arrange
            var builder = new MetricsBuilder().TimeWith.Clock(new TestClock());

            // Act
            var metrics = builder.Build();

            // Assert
            metrics.Clock.Should().BeOfType<TestClock>();
        }

        [Fact]
        public void Cannot_set_null_clock()
        {
            // Arrange
            Action action = () =>
            {
                // Act
                var unused = new MetricsBuilder().TimeWith.Clock(null);
            };

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}