// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Options;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Timer;
using App.Metrics.Timer.Abstractions;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Managers
{
    public class DefaultTimerManagerTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly MetricCoreTestFixture _fixture;
        private readonly IMeasureTimerMetrics _manager;

        public DefaultTimerManagerTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _manager = _fixture.Managers.Timer;
        }

        [Fact]
        public void can_time_action()
        {
            var metricName = "test_manager_timer_action";
            var options = new TimerOptions { Name = metricName };

            _manager.Time(options, () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 100L));

            var timerValue = _fixture.Snapshot.GetTimerValue(_fixture.Context, metricName);

            timerValue.TotalTime.Should().Be(100L);
        }

        [Fact]
        public void can_time_action_with_user_value()
        {
            var metricName = "test_manager_timer_action_with_user_value";
            var options = new TimerOptions { Name = metricName };

            _manager.Time(options, () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 100L), "value1");
            _manager.Time(options, () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 200L), "value2");

            var timerValue = _fixture.Snapshot.GetTimerValue(_fixture.Context, metricName);

            timerValue.TotalTime.Should().Be(300L);
            timerValue.Histogram.MinUserValue.Should().Be("value1");
            timerValue.Histogram.MaxUserValue.Should().Be("value2");
        }

        [Fact]
        public void can_time_in_using()
        {
            var metricName = "test_manager_timer_using";
            var options = new TimerOptions { Name = metricName };

            using (_manager.Time(options))
            {
                _fixture.Clock.Advance(TimeUnit.Milliseconds, 100L);
            }

            var timerValue = _fixture.Snapshot.GetTimerValue(_fixture.Context, metricName);

            timerValue.TotalTime.Should().Be(100L);
        }

        [Fact]
        public void can_time_in_using_with_user_value()
        {
            var metricName = "test_manager_timer_using_with_user_value";
            var options = new TimerOptions { Name = metricName };

            using (_manager.Time(options, "value1"))
            {
                _fixture.Clock.Advance(TimeUnit.Milliseconds, 100L);
            }

            using (_manager.Time(options, "value2"))
            {
                _fixture.Clock.Advance(TimeUnit.Milliseconds, 200L);
            }

            var timerValue = _fixture.Snapshot.GetTimerValue(_fixture.Context, metricName);

            timerValue.TotalTime.Should().Be(300L);
            timerValue.Histogram.MinUserValue.Should().Be("value1");
            timerValue.Histogram.MaxUserValue.Should().Be("value2");
        }
    }
}