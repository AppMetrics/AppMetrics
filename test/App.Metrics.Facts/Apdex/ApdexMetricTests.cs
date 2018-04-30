// <copyright file="ApdexMetricTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Apdex;
using App.Metrics.FactsCommon;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Metrics.Facts.Apdex
{
    public class ApdexMetricTests
    {
        private readonly DefaultApdexMetric _apdex;
        private readonly IClock _clock = new TestClock();

        public ApdexMetricTests()
        {
            _apdex = new DefaultApdexMetric(new DefaultForwardDecayingReservoir(), AppMetricsReservoirSamplingConstants.DefaultApdexTSeconds, _clock, false);
        }

        [Fact]
        public void Can_reset()
        {
            using (_apdex.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 100);
            }

            _apdex.Value.Score.Should().NotBe(0);
            _apdex.Value.SampleSize.Should().Be(1);
            _apdex.Value.Satisfied.Should().Be(1);
            _apdex.Value.Tolerating.Should().Be(0);
            _apdex.Value.Frustrating.Should().Be(0);

            _apdex.Reset();

            _apdex.Value.Score.Should().Be(0);
            _apdex.Value.SampleSize.Should().Be(0);
            _apdex.Value.Satisfied.Should().Be(0);
            _apdex.Value.Tolerating.Should().Be(0);
            _apdex.Value.Frustrating.Should().Be(0);
        }

        [Fact]
        public void Can_reset_and_get_value()
        {
            using (_apdex.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 100);
            }

            var value = _apdex.GetValue(true);
            value.Score.Should().NotBe(0);
            _apdex.Value.Score.Should().Be(0);
        }

        [Theory]
        [InlineData(1, 1.0, 600, 0.75, 10000, 0.5)]
        [InlineData(10000, 0, 1, 0.5, 600, 0.5)]
        public void Can_score(
            long durationFirstRequest,
            double apdexAfterFirstRequest,
            long durationSecondRequest,
            double apdexAfterSecondRequest,
            long durationThirdRequest,
            double apdexAfterThirdRequest)
        {
            _apdex.Value.Score.Should().Be(0);

            using (_apdex.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, durationFirstRequest);
            }

            _apdex.Value.Score.Should().Be(apdexAfterFirstRequest);

            using (_apdex.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, durationSecondRequest);
            }

            _apdex.Value.Score.Should().Be(apdexAfterSecondRequest);

            using (_apdex.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, durationThirdRequest);
            }

            _apdex.Value.Score.Should().Be(apdexAfterThirdRequest);
        }

        [Fact]
        public void Can_track_action()
        {
            _apdex.Track(() => _clock.Advance(TimeUnit.Milliseconds, 100));

            _apdex.Value.Score.Should().NotBe(0);
            _apdex.Value.SampleSize.Should().Be(1);
            _apdex.Value.Satisfied.Should().Be(1);
            _apdex.Value.Tolerating.Should().Be(0);
            _apdex.Value.Frustrating.Should().Be(0);
        }

        [Fact]
        public void Can_track_func()
        {
            var unused = _apdex.Track(
                () =>
                {
                    _clock.Advance(TimeUnit.Milliseconds, 100);
                    return 1;
                });

            _apdex.Value.Score.Should().NotBe(0);
            _apdex.Value.SampleSize.Should().Be(1);
            _apdex.Value.Satisfied.Should().Be(1);
            _apdex.Value.Tolerating.Should().Be(0);
            _apdex.Value.Frustrating.Should().Be(0);
        }

        [Fact]
        public void Context_records_only_on_first_dispose()
        {
            var context = _apdex.NewContext();
            _clock.Advance(TimeUnit.Milliseconds, 100);
            context.Dispose(); // passing the structure to using() creates a copy
            _clock.Advance(TimeUnit.Milliseconds, 100000000);
            context.Dispose();

            _apdex.Value.SampleSize.Should().Be(1);
        }

        [Fact]
        public void Context_reports_elapsed_time()
        {
            using (var context = _apdex.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 100);
                context.Elapsed.TotalMilliseconds.Should().Be(100);
            }
        }

        [Fact]
        public void Counts_even_when_action_throws()
        {
            Action action = () => _apdex.Track(() => throw new InvalidOperationException());

            action.Should().Throw<InvalidOperationException>();

            _apdex.Value.Score.Should().BeGreaterThan(0);
        }

        [Fact]
        public void If_duration_smaller_than_zero_dont_update()
        {
            var providerMock = new Mock<IApdexProvider>();

            var apdex = new DefaultApdexMetric(providerMock.Object, _clock, false);

            apdex.Track(-1L);

            providerMock.Verify(x => x.Update(0L), Times.Never);
        }

        [Fact]
        public void Throws_if_apdex_provider_is_null()
        {
            Action createApdex = () =>
            {
                var unused = new DefaultApdexMetric((IApdexProvider)null, _clock, true);
            };

            createApdex.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Throws_if_clock_is_null()
        {
            Action createApdex = () =>
            {
                var unused = new DefaultApdexMetric(
                        new DefaultForwardDecayingReservoir(),
                        AppMetricsReservoirSamplingConstants.DefaultApdexTSeconds,
                        null,
                        false)
                    ;
            };

            createApdex.Should().Throw<ArgumentNullException>();
        }
    }
}