// <copyright file="IEnvOutputFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;

namespace App.Metrics.Formatters
{
    public interface IEnvOutputFormatter
    {
        MetricsMediaTypeValue MediaType { get; }

        Task WriteAsync(
            Stream output,
            EnvironmentInfo environmentInfo,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
