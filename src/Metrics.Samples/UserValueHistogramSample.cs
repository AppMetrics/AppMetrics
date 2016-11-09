using System;
using App.Metrics;

namespace Metrics.Samples
{
    public class UserValueHistogramSample
    {
        private readonly IHistogram _histogram;

        private static IMetrics _metrics;

        public UserValueHistogramSample(IMetrics metrics)
        {
            _metrics = metrics;

            _histogram = _metrics.Advanced.Histogram(SampleMetricsRegistry.Histograms.Results);
        }

        public void Process(string documentId)
        {
            var results = GetResultsForDocument(documentId);
            _histogram.Update(results.Length, documentId);
        }

        public void RunSomeRequests()
        {
            for (var i = 0; i < 30; i++)
            {
                var documentId = new Random().Next() % 10;
                var sample = new UserValueHistogramSample(_metrics);
                sample.Process("document-" + documentId);
        }
        }

        private int[] GetResultsForDocument(string documentId)
        {
            return new int[new Random().Next() % 100];
        }
    }
}