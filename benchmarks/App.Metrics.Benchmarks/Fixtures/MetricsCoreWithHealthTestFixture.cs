// <copyright file="MetricsCoreWithHealthTestFixture.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.Benchmarks.Support;
using App.Metrics.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Benchmarks.Fixtures
{
    public class MetricsCoreWithHealthTestFixture : IDisposable
    {
#pragma warning disable SA1401
        public readonly Action ActionToTrack = () =>
        {
            // ReSharper disable UnusedVariable
            var sum = Enumerable.Range(0, 1000).Sum();
            // ReSharper restore UnusedVariable
        };
#pragma warning disable SA1401

        public readonly Random Rnd = new Random();
        private static readonly string[] UserValues = { "one", "two", "three", "four", "five" };

        public MetricsCoreWithHealthTestFixture()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddMetrics().AddHealthChecks(factory =>
            {
                factory.RegisterMetricCheck(
                    "counter metric check",
                    MetricOptions.Counter.Options,
#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
                    passing: value => (message:
                        $"OK. < 5 ({value.Count})",
                        result: value.Count < 5),
                    warning: value => (message:
                        $"WARNING. < 7 ({value.Count})"
                        , result: value.Count < 10),
                    failing: value => (message:
                        $"FAILED. >=7 ({value.Count})"
                        , result: value.Count >= 10));
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
            });

            var provider = services.BuildServiceProvider();

            Metrics = provider.GetRequiredService<IMetrics>();

            Metrics.Measure.Counter.Increment(MetricOptions.Counter.Options);
        }

        public IMetrics Metrics { get; }

        public string RandomUserValue => UserValues[Rnd.Next(UserValues.Length)];

        public void Dispose() { }
    }
}
