// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Extensions.Owin.DependencyInjection.Options;
using App.Metrics.Extensions.Owin.Internal;

// ReSharper disable CheckNamespace
namespace App.Metrics
// ReSharper restore CheckNamespace
{
    public class ApdexHealthCheck : HealthCheck
    {
        private readonly Lazy<IMetrics> _metrics;
        private readonly OwinMetricsOptions _options;

        public ApdexHealthCheck(Lazy<IMetrics> metrics, OwinMetricsOptions options)
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

            var metricsContext = _metrics.Value.Advanced.Data.ReadContext(OwinMetricsRegistry.Contexts.HttpRequests.ContextName);

            var apdex = metricsContext.ApdexValueFor(OwinMetricsRegistry.Contexts.HttpRequests.ApdexScores.ApdexMetricName);

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