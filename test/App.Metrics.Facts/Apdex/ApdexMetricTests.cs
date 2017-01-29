// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Apdex;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core.Internal;
using App.Metrics.Infrastructure;
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
            var reservoir = new Lazy<IReservoir>(() => new DefaultForwardDecayingReservoir());

            _apdex = new DefaultApdexMetric(reservoir, Constants.ReservoirSampling.DefaultApdexTSeconds, _clock, false);
        }

        [Fact]
        public void can_reset()
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
        public void can_reset_and_get_value()
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
        public void can_score(
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
        public void can_track_action()
        {
            _apdex.Track(() => _clock.Advance(TimeUnit.Milliseconds, 100));

            _apdex.Value.Score.Should().NotBe(0);
            _apdex.Value.SampleSize.Should().Be(1);
            _apdex.Value.Satisfied.Should().Be(1);
            _apdex.Value.Tolerating.Should().Be(0);
            _apdex.Value.Frustrating.Should().Be(0);
        }

        [Fact]
        public void can_track_func()
        {
            var result = _apdex.Track(
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
        public void context_records_only_on_first_dispose()
        {
            var context = _apdex.NewContext();
            _clock.Advance(TimeUnit.Milliseconds, 100);
            context.Dispose(); // passing the structure to using() creates a copy
            _clock.Advance(TimeUnit.Milliseconds, 100000000);
            context.Dispose();

            _apdex.Value.SampleSize.Should().Be(1);
        }

        [Fact]
        public void context_reports_elapsed_time()
        {
            using (var context = _apdex.NewContext())
            {
                _clock.Advance(TimeUnit.Milliseconds, 100);
                context.Elapsed.TotalMilliseconds.Should().Be(100);
            }
        }

        [Fact]
        public void counts_even_when_action_throws()
        {
            //Action action = () => this._apdex.Time(() => { throw new InvalidOperationException(); });

            //action.ShouldThrow<InvalidOperationException>();

            //this._apdex.Value.Score.Should().BeGreaterThan(0);
        }

        [Fact]
        public void if_duration_smaller_than_zero_dont_update()
        {
            var providerMock = new Mock<IApdexProvider>();

            var apdex = new DefaultApdexMetric(providerMock.Object, _clock, false);

            apdex.Track(-1L);

            providerMock.Verify(x => x.Update(0L), Times.Never);
        }

        [Fact]
        public void throws_if_apdex_provider_is_null()
        {
            IApdexProvider provider = null;
            Action createApdex = () =>
            {
                var apdex = new DefaultApdexMetric(provider, _clock, true);
            };

            createApdex.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void throws_if_clock_is_null()
        {
            Action createApdex = () =>
            {
                var reservoir = new Lazy<IReservoir>(() => new DefaultForwardDecayingReservoir());
                var apdex = new DefaultApdexMetric(
                        reservoir,
                        Constants.ReservoirSampling.DefaultApdexTSeconds,
                        null,
                        false)
                    ;
            };

            createApdex.ShouldThrow<ArgumentNullException>();
        }
    }
}