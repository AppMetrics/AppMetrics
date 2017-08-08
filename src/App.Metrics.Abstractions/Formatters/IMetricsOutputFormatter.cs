// <copyright file="IMetricsOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Formatters
{
    public interface IMetricsOutputFormatter
    {
        MetricsMediaTypeValue MediaType { get; }

        Task WriteAsync(
            Stream output,
            MetricsDataValueSource metricsData,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}