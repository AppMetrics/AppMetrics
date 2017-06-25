// <copyright file="MetricsCoreTestFixture.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Benchmarks.Fixtures
{
    public class MetricsCoreTestFixture : IDisposable
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

        public MetricsCoreTestFixture()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddMetrics();

            var provider = services.BuildServiceProvider();

            Metrics = provider.GetRequiredService<IMetrics>();
        }

        public IMetrics Metrics { get; }

        public string RandomUserValue => UserValues[Rnd.Next(UserValues.Length)];

        public void Dispose() { }
    }
}