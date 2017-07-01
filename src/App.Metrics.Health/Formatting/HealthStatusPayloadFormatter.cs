// <copyright file="HealthStatusPayloadFormatter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Linq;

namespace App.Metrics.Health.Formatting
{
    public class HealthStatusPayloadFormatter
    {
        public void Build<TPayload>(HealthStatus healthStatus, IHealthStatusPayloadBuilder<TPayload> payloadBuilder)
        {
            var passed = healthStatus.Results.Where(r => r.Check.Status.IsHealthy()).ToList();
            var failed = healthStatus.Results.Where(r => r.Check.Status.IsUnhealthy()).ToList();
            var degraded = healthStatus.Results.Where(r => r.Check.Status.IsDegraded()).ToList();
            var ignored = healthStatus.Results.Where(r => r.Check.Status.IsIgnored()).ToList();

            passed.ForEach(c => payloadBuilder.Pack(c.Name, c.Check.Message, c.Check.Status));
            degraded.ForEach(c => payloadBuilder.Pack(c.Name, c.Check.Message, c.Check.Status));
            failed.ForEach(c => payloadBuilder.Pack(c.Name, c.Check.Message, c.Check.Status));
            ignored.ForEach(c => payloadBuilder.Pack(c.Name, c.Check.Message, c.Check.Status));
        }
    }
}