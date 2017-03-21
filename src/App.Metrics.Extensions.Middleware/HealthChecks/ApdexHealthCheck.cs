// <copyright file="ApdexHealthCheck.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Apdex;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using App.Metrics.Extensions.Middleware.Internal;
using App.Metrics.Health;

// ReSharper disable CheckNamespace
namespace App.Metrics
{
    // ReSharper restore CheckNamespace
    public class ApdexHealthCheck : HealthCheck
    {
        private readonly string _context = HttpRequestMetricsRegistry.ContextName;
        private readonly string _metricName = HttpRequestMetricsRegistry.ApdexScores.ApdexMetricName;
        private readonly Lazy<IMetrics> _metrics;
        private readonly AspNetMetricsOptions _options;

        public ApdexHealthCheck(Lazy<IMetrics> metrics, AspNetMetricsOptions options)
            : base("Apdex Score")
        {
            _metrics = metrics;
            _options = options;
        }

        protected override Task<HealthCheckResult> CheckAsync(CancellationToken token = default(CancellationToken))
        {
            if (!_options.ApdexTrackingEnabled)
            {
                return Task.FromResult(HealthCheckResult.Ignore());
            }

            var apdex = _metrics.Value.Snapshot.GetApdexValue(_context, _metricName);

            if (apdex.Score < 0.5)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy($"Frustrating. Score: {apdex.Score}"));
            }

            if (apdex.Score >= 0.5 && apdex.Score < 0.75)
            {
                return Task.FromResult(HealthCheckResult.Degraded($"Tolerating. Score: {apdex.Score}"));
            }

            return Task.FromResult(HealthCheckResult.Healthy($"Satisfied. Score {apdex.Score}"));
        }
    }
}