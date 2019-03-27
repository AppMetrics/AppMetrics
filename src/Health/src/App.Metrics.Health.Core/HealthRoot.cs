// <copyright file="HealthRoot.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Health.Formatters;
using App.Metrics.Health.Internal;

namespace App.Metrics.Health
{
    public class HealthRoot : IHealthRoot
    {
        private readonly IHealth _health;

        public HealthRoot(
            IHealth health,
            HealthOptions options,
            HealthFormatterCollection metricsOutputFormatters,
            IHealthOutputFormatter defaultMetricsOutputFormatter,
            IRunHealthChecks healthCheckRunner,
            HealthReporterCollection reporterCollection)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            _health = health ?? throw new ArgumentNullException(nameof(health));
            OutputHealthFormatters = metricsOutputFormatters ?? new HealthFormatterCollection();
            DefaultOutputHealthFormatter = defaultMetricsOutputFormatter;
            HealthCheckRunner = healthCheckRunner;
            Reporters = reporterCollection ?? new HealthReporterCollection();
        }

        /// <inheritdoc />
        public IReadOnlyCollection<IHealthOutputFormatter> OutputHealthFormatters { get; }

        /// <inheritdoc />
        public IHealthOutputFormatter DefaultOutputHealthFormatter { get; }

        /// <inheritdoc />
        public HealthOptions Options { get; }

        /// <inheritdoc />
        public IRunHealthChecks HealthCheckRunner { get; }

        /// <inheritdoc />
        public IReadOnlyCollection<IReportHealthStatus> Reporters { get; }

        /// <inheritdoc />
        public IEnumerable<HealthCheck> Checks => _health.Checks;
    }
}