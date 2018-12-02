// <copyright file="ExponentiallyDecayingReservoirTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.FactsCommon;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.Scheduling;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Sampling.Facts
{
    public class ExponentiallyDecayingReservoirTests
    {
        private readonly IClock _clock;
        private readonly IReservoirRescaleScheduler _scheduler;

        public ExponentiallyDecayingReservoirTests()
        {
            _clock = new TestClock();
            _scheduler = new TestReservoirRescaleScheduler(_clock);
        }

        [Fact]
        public void EDR_HeavilyBiasedReservoirOf100OutOf1000Elements()
        {
            var reservoir = new DefaultForwardDecayingReservoir(1000, 0.01);
            for (var i = 0; i < 100; i++)
            {
                reservoir.Update(i);
            }

            reservoir.Size.Should().Be(100);
            var snapshot = reservoir.GetSnapshot();
            snapshot.Size.Should().Be(100);
            snapshot.Values.Should().OnlyContain(v => v >= 0 && v < 100);
        }

        [Fact]
        public void EDR_longPeriodsOfInactivityShouldNotCorruptSamplingState()
        {
            var reservoir = new DefaultForwardDecayingReservoir(
                10,
                0.015,
                0.0,
                _clock,
                _scheduler);

            // add 1000 values at a rate of 10 values/second
            for (var i = 0; i < 1000; i++)
            {
                reservoir.Update(1000 + i);
                _clock.Advance(TimeUnit.Milliseconds, 100);
            }

            reservoir.GetSnapshot().Size.Should().Be(10);
            reservoir.GetSnapshot().Values.Should().OnlyContain(v => v >= 1000 && v < 2000);

            // wait for 15 hours and add another value.
            // this should trigger a rescale. Note that the number of samples will be reduced to 2
            // because of the very small scaling factor that will make all existing priorities equal to
            // zero after rescale.
            _clock.Advance(TimeUnit.Hours, 15);
            reservoir.Update(2000);
            var snapshot = reservoir.GetSnapshot();
            snapshot.Size.Should().Be(2);
            snapshot.Values.Should().OnlyContain(v => v >= 1000 && v < 3000);
            snapshot.Sum.Should().Be(snapshot.Values.Sum());

            // add 1000 values at a rate of 10 values/second
            for (var i = 0; i < 1000; i++)
            {
                reservoir.Update(3000 + i);
                _clock.Advance(TimeUnit.Milliseconds, 100);
            }

            var finalSnapshot = reservoir.GetSnapshot();

            finalSnapshot.Size.Should().Be(10);
            snapshot.Sum.Should().Be(snapshot.Values.Sum());
            finalSnapshot.Values.Skip(1).Should().OnlyContain(v => v >= 3000 && v < 4000);
        }

        [Fact]
        public void EDR_QuantiliesShouldBeBasedOnWeights()
        {
            var reservoir = new DefaultForwardDecayingReservoir(
                AppMetricsReservoirSamplingConstants.DefaultSampleSize,
                AppMetricsReservoirSamplingConstants.DefaultExponentialDecayFactor,
                AppMetricsReservoirSamplingConstants.DefaultMinimumSampleWeight,
                _clock,
                _scheduler);

            for (var i = 0; i < 40; i++)
            {
                reservoir.Update(177);
            }

            _clock.Advance(TimeUnit.Seconds, 120);

            for (var i = 0; i < 10; i++)
            {
                reservoir.Update(9999);
            }

            reservoir.GetSnapshot().Size.Should().Be(50);

            // the first added 40 items (177) have weights 1
            // the next added 10 items (9999) have weights ~6
            // so, it's 40 vs 60 distribution, not 40 vs 10
            reservoir.GetSnapshot().Median.Should().Be(9999);
            reservoir.GetSnapshot().Percentile75.Should().Be(9999);
        }

        [Fact]
        public void EDR_RecordsUserValue()
        {
            var reservoir = new DefaultForwardDecayingReservoir(
                AppMetricsReservoirSamplingConstants.DefaultSampleSize,
                AppMetricsReservoirSamplingConstants.DefaultExponentialDecayFactor,
                AppMetricsReservoirSamplingConstants.DefaultMinimumSampleWeight,
                _clock,
                _scheduler);

            reservoir.Update(2L, "B");
            reservoir.Update(1L, "A");

            reservoir.GetSnapshot().MinUserValue.Should().Be("A");
            reservoir.GetSnapshot().MaxUserValue.Should().Be("B");
        }

        [Fact]
        public void EDR_ReservoirOf100OutOf1000Elements()
        {
            var reservoir = new DefaultForwardDecayingReservoir(100, 0.99);
            for (var i = 0; i < 1000; i++)
            {
                reservoir.Update(i);
            }

            reservoir.Size.Should().Be(100);
            var snapshot = reservoir.GetSnapshot();
            snapshot.Size.Should().Be(100);
            snapshot.Values.Should().OnlyContain(v => v >= 0 && v < 1000);
        }

        [Fact]
        public void EDR_ReservoirOf100OutOf10Elements()
        {
            var reservoir = new DefaultForwardDecayingReservoir(100, 0.99);
            for (var i = 0; i < 10; i++)
            {
                reservoir.Update(i);
            }

            reservoir.Size.Should().Be(10);
            var snapshot = reservoir.GetSnapshot();
            snapshot.Size.Should().Be(10);
            snapshot.Values.Should().OnlyContain(v => v >= 0 && v < 10);
        }

        [Fact]
        public void EDR_SpotFall()
        {
            var reservoir = new DefaultForwardDecayingReservoir(
                AppMetricsReservoirSamplingConstants.DefaultSampleSize,
                AppMetricsReservoirSamplingConstants.DefaultExponentialDecayFactor,
                AppMetricsReservoirSamplingConstants.DefaultMinimumSampleWeight,
                _clock,
                _scheduler);

            var valuesRatePerMinute = 10;
            var valuesIntervalMillis = (int)(TimeUnit.Minutes.ToMilliseconds(1) / valuesRatePerMinute);
            // mode 1: steady regime for 120 minutes
            for (var i = 0; i < 120 * valuesRatePerMinute; i++)
            {
                reservoir.Update(9998);
                _clock.Advance(TimeUnit.Milliseconds, valuesIntervalMillis);
            }

            // switching to mode 2: 10 minutes more with the same rate, but smaller value
            for (var i = 0; i < 10 * valuesRatePerMinute; i++)
            {
                reservoir.Update(178);
                _clock.Advance(TimeUnit.Milliseconds, valuesIntervalMillis);
            }

            // expect that quantiles should be more about mode 2 after 10 minutes
            reservoir.GetSnapshot().Percentile95.Should().Be(178);
        }

        [Fact]
        public void EDR_SpotLift()
        {
            var reservoir = new DefaultForwardDecayingReservoir(
                AppMetricsReservoirSamplingConstants.DefaultSampleSize,
                AppMetricsReservoirSamplingConstants.DefaultExponentialDecayFactor,
                AppMetricsReservoirSamplingConstants.DefaultMinimumSampleWeight,
                _clock,
                _scheduler);

            var valuesRatePerMinute = 10;
            var valuesIntervalMillis = (int)(TimeUnit.Minutes.ToMilliseconds(1) / valuesRatePerMinute);
            // mode 1: steady regime for 120 minutes
            for (var i = 0; i < 120 * valuesRatePerMinute; i++)
            {
                reservoir.Update(177);
                _clock.Advance(TimeUnit.Milliseconds, valuesIntervalMillis);
            }

            // switching to mode 2: 10 minutes more with the same rate, but larger value
            for (var i = 0; i < 10 * valuesRatePerMinute; i++)
            {
                reservoir.Update(9999);
                _clock.Advance(TimeUnit.Milliseconds, valuesIntervalMillis);
            }

            // expect that quantiles should be more about mode 2 after 10 minutes
            reservoir.GetSnapshot().Median.Should().Be(9999);
        }

        [Fact]
        public void EDR_NonzeroMinimumSampleWeightEvictsSamplesDuringRescale()
        {
            var reservoir = new DefaultForwardDecayingReservoir(
                10,
                AppMetricsReservoirSamplingConstants.DefaultExponentialDecayFactor,
                0.1,
                _clock,
                new TestReservoirRescaleScheduler(_clock, TimeSpan.FromSeconds(200)));

            // First set of values will have the weight equal to 1
            for (var i = 0; i < 5; i++)
            {
                reservoir.Update(100);
            }

            _clock.Advance(TimeUnit.Seconds, 120);

            // The second set of values will have weights equal to about 6
            for (var i = 0; i < 5; i++)
            {
                reservoir.Update(1000);
            }

            reservoir.GetSnapshot().Size.Should().Be(10);

            // Trigger rescale
            _clock.Advance(TimeUnit.Seconds, 120);

            // New weights for the two value sets will be about 0.03 and 0.16 respectively,
            // so a minimum weight of 0.1 should have eliminated the first set.
            reservoir.GetSnapshot().Size.Should().Be(5);
            reservoir.GetSnapshot().Values.Should().OnlyContain(v => v == 1000);
        }
    }
}