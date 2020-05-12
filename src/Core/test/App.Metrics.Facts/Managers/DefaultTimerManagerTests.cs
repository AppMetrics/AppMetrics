// <copyright file="DefaultTimerManagerTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Facts.Fixtures;
using App.Metrics.Timer;
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
        public void Can_time_action()
        {
            var metricName = "test_manager_timer_action";
            var options = new TimerOptions { Name = metricName };

            _manager.Time(options, () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 100L));

            var timerValue = _fixture.Snapshot.GetTimerValue(_fixture.Context, metricName);

            timerValue.Histogram.Sum.Should().Be(100.0);
        }

        [Fact]
        public void Can_time_action_when_multidimensional()
        {
            var metricName = "test_manager_timer_action_multi";
            var options = new TimerOptions { Name = metricName };

            _manager.Time(options, _fixture.Tags[0], () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 100L));
            _manager.Time(options, _fixture.Tags[1], () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 1000L));

            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[0].AsMetricName(metricName)).Histogram.Sum.Should().Be(100.0);
            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[1].AsMetricName(metricName)).Histogram.Sum.Should().Be(1000.0);
        }

        [Fact]
        public void Can_time_action_when_multidimensional_with_user_value()
        {
            var metricName = "test_manager_timer_action_with_user_value_multi";
            var options = new TimerOptions { Name = metricName };

            _manager.Time(options, _fixture.Tags[0], () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 100L), "value1");
            _manager.Time(options, _fixture.Tags[0], () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 200L), "value2");
            _manager.Time(options, _fixture.Tags[1], () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 1000L), "value1");
            _manager.Time(options, _fixture.Tags[1], () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 2000L), "value2");

            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[0].AsMetricName(metricName)).Histogram.Sum.Should().Be(300.0);
            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[0].AsMetricName(metricName)).Histogram.MinUserValue.Should().Be("value1");
            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[0].AsMetricName(metricName)).Histogram.MaxUserValue.Should().Be("value2");

            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[1].AsMetricName(metricName)).Histogram.Sum.Should().Be(3000.0);
            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[1].AsMetricName(metricName)).Histogram.MinUserValue.Should().Be("value1");
            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[1].AsMetricName(metricName)).Histogram.MaxUserValue.Should().Be("value2");
        }

        [Fact]
        public void Can_time_action_with_user_value()
        {
            var metricName = "test_manager_timer_action_with_user_value";
            var options = new TimerOptions { Name = metricName };

            _manager.Time(options, () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 100L), "value1");
            _manager.Time(options, () => _fixture.Clock.Advance(TimeUnit.Milliseconds, 200L), "value2");

            var timerValue = _fixture.Snapshot.GetTimerValue(_fixture.Context, metricName);

            timerValue.Histogram.Sum.Should().Be(300.0);
            timerValue.Histogram.MinUserValue.Should().Be("value1");
            timerValue.Histogram.MaxUserValue.Should().Be("value2");
        }

        [Fact]
        public void Can_time_in_using()
        {
            var metricName = "test_manager_timer_using";
            var options = new TimerOptions { Name = metricName };

            using (_manager.Time(options))
            {
                _fixture.Clock.Advance(TimeUnit.Milliseconds, 100L);
            }

            var timerValue = _fixture.Snapshot.GetTimerValue(_fixture.Context, metricName);

            timerValue.Histogram.Sum.Should().Be(100.0);
        }

        [Fact]
        public void Can_time_with_specified_value()
        {
            var metricName = "test_manager_timer_specified_value";
            var options = new TimerOptions { Name = metricName };

            _manager.Time(options, 250);

            var timerValue = _fixture.Snapshot.GetTimerValue(_fixture.Context, metricName);

            timerValue.Histogram.Sum.Should().Be(250.0);
        }

        [Fact]
        public void Can_time_in_using_when_multidimensional()
        {
            var metricName = "test_manager_timer_using_multi";
            var options = new TimerOptions { Name = metricName };

            using (_manager.Time(options, _fixture.Tags[0]))
            {
                _fixture.Clock.Advance(TimeUnit.Milliseconds, 100L);
            }

            using (_manager.Time(options, _fixture.Tags[1]))
            {
                _fixture.Clock.Advance(TimeUnit.Milliseconds, 500L);
            }

            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[0].AsMetricName(metricName)).Histogram.Sum.Should().Be(100.0);
            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[1].AsMetricName(metricName)).Histogram.Sum.Should().Be(500.0);
        }

        [Fact]
        public void Can_time_in_using_when_multidimensional_with_user_value()
        {
            var metricName = "test_manager_timer_using_with_user_value_multi";
            var options = new TimerOptions { Name = metricName };

            using (_manager.Time(options, _fixture.Tags[0], "value1"))
            {
                _fixture.Clock.Advance(TimeUnit.Milliseconds, 100L);
            }

            using (_manager.Time(options, _fixture.Tags[0], "value2"))
            {
                _fixture.Clock.Advance(TimeUnit.Milliseconds, 200L);
            }

            using (_manager.Time(options, _fixture.Tags[1], "value1"))
            {
                _fixture.Clock.Advance(TimeUnit.Milliseconds, 1000L);
            }

            using (_manager.Time(options, _fixture.Tags[1], "value2"))
            {
                _fixture.Clock.Advance(TimeUnit.Milliseconds, 2000L);
            }

            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[0].AsMetricName(metricName)).Histogram.Sum.Should().Be(300.0);
            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[0].AsMetricName(metricName)).Histogram.MinUserValue.Should().Be("value1");
            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[0].AsMetricName(metricName)).Histogram.MaxUserValue.Should().Be("value2");

            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[1].AsMetricName(metricName)).Histogram.Sum.Should().Be(3000.0);
            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[1].AsMetricName(metricName)).Histogram.MinUserValue.Should().Be("value1");
            _fixture.Snapshot.GetTimerValue(_fixture.Context, _fixture.Tags[1].AsMetricName(metricName)).Histogram.MaxUserValue.Should().Be("value2");
        }

        [Fact]
        public void Can_time_in_using_with_user_value()
        {
            var metricName = "test_manager_timer_using_with_user_value_multi";
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

            timerValue.Histogram.Sum.Should().Be(300.0);
            timerValue.Histogram.MinUserValue.Should().Be("value1");
            timerValue.Histogram.MaxUserValue.Should().Be("value2");
        }
    }
}