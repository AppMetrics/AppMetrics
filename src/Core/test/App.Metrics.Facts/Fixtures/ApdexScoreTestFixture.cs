// <copyright file="ApdexScoreTestFixture.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.Facts.Apdex;
using App.Metrics.FactsCommon;
using App.Metrics.ReservoirSampling.ExponentialDecay;

namespace App.Metrics.Facts.Fixtures
{
    public class ApdexScoreTestFixture : IDisposable
    {
        public void Dispose() { }

        public IApdexMetric RunSamplesForApdexCalculation(
            double apdexTSeconds,
            int satisifedRequests,
            int toleratingRequests,
            int frustratingRequest,
            TestSamplePreference testSamplePreference)
        {
            var maxSatifiedDurationMilliseconds = (int)(apdexTSeconds * 1000);
            var minToleratedDurationMilliseconds = maxSatifiedDurationMilliseconds + 1;
            var maxToleratedDurationMilliseconds = 4 * (int)(apdexTSeconds * 1000);
            var minFrustratedDurationMilliseconds = maxToleratedDurationMilliseconds + 1;
            var clock = new TestClock();
            var random = new Random();

            var satisfiedRequestsDurations = Enumerable.Range(1, satisifedRequests).Select(x => random.Next(1, maxSatifiedDurationMilliseconds));
            var toleratingRequestsDurations =
                Enumerable.Range(1, toleratingRequests).Select(x => random.Next(minToleratedDurationMilliseconds, maxToleratedDurationMilliseconds));
            var frustratingRequestsDurations =
                Enumerable.Range(1, frustratingRequest).Select(
                    x => random.Next(minFrustratedDurationMilliseconds, minFrustratedDurationMilliseconds * 2));

            var apdexMetric = new DefaultApdexMetric(new DefaultForwardDecayingReservoir(), apdexTSeconds, clock, false);

            if (testSamplePreference == TestSamplePreference.Satisified)
            {
                RunSamples(satisfiedRequestsDurations, apdexMetric, clock);
                RunSamples(toleratingRequestsDurations, apdexMetric, clock);
                RunSamples(frustratingRequestsDurations, apdexMetric, clock);
            }
            else if (testSamplePreference == TestSamplePreference.Frustrating)
            {
                RunSamples(frustratingRequestsDurations, apdexMetric, clock);
                RunSamples(toleratingRequestsDurations, apdexMetric, clock);
                RunSamples(satisfiedRequestsDurations, apdexMetric, clock);
            }
            else
            {
                RunSamples(toleratingRequestsDurations, apdexMetric, clock);
                RunSamples(frustratingRequestsDurations, apdexMetric, clock);
                RunSamples(satisfiedRequestsDurations, apdexMetric, clock);
            }

            return apdexMetric;
        }

        private static void RunSamples(IEnumerable<int> satisfiedRequestsDurations, IApdexMetric apdexMetric, TestClock clock)
        {
            foreach (var duration in satisfiedRequestsDurations)
            {
                using (apdexMetric.NewContext())
                {
                    clock.Advance(TimeUnit.Milliseconds, duration);
                }
            }
        }
    }
}