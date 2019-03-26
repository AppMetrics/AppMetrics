// <copyright file="UserValueHistogramSample.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.Histogram;

namespace ReportingSandbox.JustForTesting
{
    public class UserValueHistogramSample
    {
        private static IMetrics _metrics;
        private readonly IHistogram _histogram;

        public UserValueHistogramSample(IMetrics metrics)
        {
            _metrics = metrics;

            _histogram = _metrics.Provider.Histogram.Instance(SampleMetricsRegistry.Histograms.Results);
        }

        public void Run()
        {
            for (var i = 0; i < 30; i++)
            {
                var documentId = new Random().Next() % 10;
                var sample = new UserValueHistogramSample(_metrics);
                sample.Process("document-" + documentId);
            }
        }

        private int[] GetResultsForDocument() { return new int[new Random().Next() % 100]; }

        private void Process(string documentId)
        {
            var results = GetResultsForDocument();
            _histogram.Update(results.Length, documentId);
        }
    }
}