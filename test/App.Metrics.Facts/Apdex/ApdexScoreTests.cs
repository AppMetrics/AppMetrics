// <copyright file="ApdexScoreTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.Facts.Fixtures;
using App.Metrics.FactsCommon;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Apdex
{
    public class ApdexScoreTests : IClassFixture<ApdexScoreTestFixture>
    {
        private readonly ApdexScoreTestFixture _fixture;

        public ApdexScoreTests(ApdexScoreTestFixture fixture) { _fixture = fixture; }

        [Fact]
        public void Apdex_score_should_be_between_zero_and_one()
        {
            const double apdexTSeconds = 0.5;
            const int fromMilliSeconds = 20;
            const int toMilliSeconds = 5000;
            var random = new Random();
            var clock = new TestClock();

            IApdexMetric apdexMetric = new DefaultApdexMetric(new DefaultForwardDecayingReservoir(), apdexTSeconds, clock, false);

            foreach (var unused in Enumerable.Range(0, 1000))
            {
                using (apdexMetric.NewContext())
                {
                    clock.Advance(TimeUnit.Milliseconds, random.Next(fromMilliSeconds, toMilliSeconds));
                }
            }

            var score = apdexMetric.GetValue().Score;

            score.Should().BeGreaterOrEqualTo(0);
            score.Should().BeLessOrEqualTo(1);
        }

        [Theory]
        [InlineData(0.1, 1, 3, 1, 0.5)]
        [InlineData(0.1, 1, 0, 0, 1)]
        [InlineData(0.5, 170, 20, 10, 0.9)]
        [InlineData(0.5, 340, 40, 20, 0.9)]
        [InlineData(1.0, 250, 800, 60, 0.59)]
        [InlineData(3.0, 60, 30, 10, 0.75)]
        public void Can_calculate_apdex_score(
            double apdexTSeconds,
            int satisifedRequests,
            int toleratingRequests,
            int frustratingRequest,
            double expected)
        {
            var apdexMetric = _fixture.RunSamplesForApdexCalculation(
                apdexTSeconds,
                satisifedRequests,
                toleratingRequests,
                frustratingRequest,
                TestSamplePreference.Satisified);

            var score = apdexMetric.GetValue().Score;

            score.Should().BeApproximately(expected, 2);
        }

        [Theory]
        [InlineData(0.5, 3000, 1000, 5000, 0.5)]
        public void Recent_failures_should_reduce_apdex(
            double apdexTSeconds,
            int satisifedRequests,
            int toleratingRequests,
            int frustratingRequest,
            double expectedLessThan)
        {
            var apdexMetric = _fixture.RunSamplesForApdexCalculation(
                apdexTSeconds,
                satisifedRequests,
                toleratingRequests,
                frustratingRequest,
                TestSamplePreference.Frustrating);

            var score = apdexMetric.GetValueOrDefault().Score;

            score.Should().BeLessThan(expectedLessThan);
        }

        [Theory]
        [InlineData(0.5, 3000, 1000, 5000, 0.5)]
        public void Recent_satisifed_should_increase_apdex(
            double apdexTSeconds,
            int satisifedRequests,
            int toleratingRequests,
            int frustratingRequest,
            double expectedGreaterThan)
        {
            var apdexMetric = _fixture.RunSamplesForApdexCalculation(
                apdexTSeconds,
                satisifedRequests,
                toleratingRequests,
                frustratingRequest,
                TestSamplePreference.Satisified);

            var score = apdexMetric.GetValue().Score;

            score.Should().BeLessThan(expectedGreaterThan);
        }
    }
}