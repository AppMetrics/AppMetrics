// <copyright file="NoOpHealthProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

// ReSharper disable CheckNamespace
namespace App.Metrics.Core.Internal
    // ReSharper restore CheckNamespace
{
    internal sealed class NoOpHealthProvider : IProvideHealth
    {
        /// <inheritdoc />
        public ValueTask<HealthStatus> ReadStatusAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return new ValueTask<HealthStatus>(new HealthStatus(Enumerable.Empty<HealthCheck.Result>()));
        }
    }
}
