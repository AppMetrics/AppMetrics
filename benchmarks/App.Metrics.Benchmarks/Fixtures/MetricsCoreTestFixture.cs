// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Benchmarks.Fixtures
{
    public class MetricsCoreTestFixture : IDisposable
    {
        public readonly Action ActionToTrack = () =>
        {
            var sum = Enumerable.Range(0, 1000).Sum();
        };

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