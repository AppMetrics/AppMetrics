// <copyright file="MetricsCoreTestFixture.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;

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
            Metrics = new MetricsBuilder()
                .Configuration.Configure(options =>
                {
                    options.DefaultContextLabel = "Testing";
                    options.Enabled = true;
                })
                .OutputEnvInfo.AsJson()
                .OutputEnvInfo.AsPlainText()
                .OutputMetrics.AsPlainText()
                .OutputMetrics.AsJson()
                .SampleWith.AlgorithmR()
                .Filter.ByIncludingOnlyTypes(MetricType.Counter)
                .TimeWith.StopwatchClock()
                .Build();
        }

        public IMetrics Metrics { get; }

        public string RandomUserValue => UserValues[Rnd.Next(UserValues.Length)];

        public void Dispose() { }
    }
}