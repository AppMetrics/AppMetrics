// <copyright file="HealthBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Metrics.Health.Formatters;
using App.Metrics.Health.Formatters.Ascii;
using App.Metrics.Health.Internal;
using App.Metrics.Health.Internal.NoOp;
using App.Metrics.Health.Logging;

namespace App.Metrics.Health.Builder
{
    public class HealthBuilder : IHealthBuilder
    {
        private static readonly ILog Logger = LogProvider.For<HealthBuilder>();
        private readonly Dictionary<string, HealthCheck> _checks = new Dictionary<string, HealthCheck>(StringComparer.OrdinalIgnoreCase);
        private readonly HealthFormatterCollection _healthFormatterCollection = new HealthFormatterCollection();
        private readonly HealthReporterCollection _healthStatusReporters = new HealthReporterCollection();
        private IHealthOutputFormatter _defaultMetricsHealthFormatter;
        private HealthOptions _options;

        /// <inheritdoc />
        public bool CanReport() { return _options.Enabled && _options.ReportingEnabled && _healthStatusReporters.Any(); }

        /// <inheritdoc />
        public IHealthConfigurationBuilder Configuration
        {
            get
            {
                return new HealthConfigurationBuilder(
                    this,
                    _options,
                    options => { _options = options; });
            }
        }

        public IHealthCheckBuilder HealthChecks
        {
            get
            {
                return new HealthCheckBuilder(
                    this,
                    healthCheck =>
                    {
                        try
                        {
                            _checks.Add(healthCheck.Name, healthCheck);
                        }
                        catch (ArgumentException ex)
                        {
                            Logger.Error(ex, $"Attempted to add health checks with duplicates names: {healthCheck.Name}");
                            throw;
                        }
                    });
            }
        }

        /// <inheritdoc />
        public IHealthOutputFormattingBuilder OutputHealth => new HealthOutputFormattingBuilder(
            this,
            (replaceExisting, formatter) =>
            {
                if (_defaultMetricsHealthFormatter == null)
                {
                    _defaultMetricsHealthFormatter = formatter;
                }

                if (replaceExisting)
                {
                    _healthFormatterCollection.TryAdd(formatter);
                }
                else
                {
                    if (_healthFormatterCollection.GetType(formatter.GetType()) == null)
                    {
                        _healthFormatterCollection.Add(formatter);
                    }
                }
            });

        /// <inheritdoc />
        public IHealthReportingBuilder Report => new HealthReportingBuilder(
            this,
            reporter => { _healthStatusReporters.TryAdd(reporter); });

        public IHealthRoot Build()
        {
            if (_options == null)
            {
                _options = new HealthOptions();
            }

            if (_healthFormatterCollection.Count == 0)
            {
                _healthFormatterCollection.Add(new HealthStatusTextOutputFormatter());
            }

            IRunHealthChecks healthCheckRunner;

            var health = new DefaultHealth(_checks.Values);
            var defaultMetricsOutputFormatter = _defaultMetricsHealthFormatter ?? _healthFormatterCollection.FirstOrDefault();

            if (_options.Enabled && health.Checks.Any())
            {
                healthCheckRunner = new DefaultHealthCheckRunner(health.Checks);
            }
            else
            {
                healthCheckRunner = new NoOpHealthCheckRunner();
            }

            if (string.IsNullOrWhiteSpace(_options.ApplicationName))
            {
                var entryAssembly = Assembly.GetEntryAssembly();

                _options.ApplicationName = entryAssembly?.GetName()?.Name?.Trim();
            }

            return new HealthRoot(
                health,
                _options,
                _healthFormatterCollection,
                defaultMetricsOutputFormatter,
                healthCheckRunner,
                _healthStatusReporters);
        }
    }
}