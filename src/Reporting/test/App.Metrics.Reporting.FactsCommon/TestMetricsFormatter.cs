// <copyright file="TestMetricsFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Formatters;

namespace App.Metrics.Reporting.FactsCommon
{
    public class TestMetricsFormatter : IMetricsOutputFormatter
    {
        public TestMetricsFormatter()
        {
            MediaType = new MetricsMediaTypeValue("test", "test", "v1", "format");
            MetricFields = new MetricFields();
        }

        public Task WriteAsync(Stream output, MetricsDataValueSource metricsData, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public MetricsMediaTypeValue MediaType { get; }

        /// <inheritdoc />
        public MetricFields MetricFields { get; set; }
    }
}
