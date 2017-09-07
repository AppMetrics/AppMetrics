// <copyright file="MetricsBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Linq;
using App.Metrics.Filtering;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Internal.NoOp;
using App.Metrics.Registry;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public class MetricsBuilder : IMetricsBuilder
    {
        private readonly MetricsFormatterCollection _metricsOutputFormatters = new MetricsFormatterCollection();
        private readonly EnvFormatterCollection _envFormatters = new EnvFormatterCollection();
        private readonly EnvironmentInfoProvider _environmentInfoProvider = new EnvironmentInfoProvider();
        private IMetricsOutputFormatter _defaultMetricsOutputFormatter;
        private IEnvOutputFormatter _defauEnvOutputFormatter;
        private DefaultSamplingReservoirProvider _defaultSamplingReservoir = new DefaultSamplingReservoirProvider(() => new DefaultForwardDecayingReservoir());
        private IFilterMetrics _metricsFilter = new NoOpMetricsFilter();
        private IClock _clock = new StopwatchClock();
        private MetricsOptions _options;

        /// <inheritdoc />
        public IMetricsFilterBuilder Filter
        {
            get
            {
                return new MetricsFilterBuilder(
                    this,
                    metricsFilter =>
                    {
                        _metricsFilter = metricsFilter;
                    });
            }
        }

        /// <inheritdoc />
        public IMetricsClockBuilder TimeWith
        {
            get
            {
                return new MetricsClockBuilder(
                    this,
                    clock =>
                    {
                        _clock = clock;
                    });
            }
        }

        /// <inheritdoc />
        public IMetricsOutputFormattingBuilder OutputMetrics => new MetricsOutputFormattingBuilder(this, formatter =>
        {
            if (_defaultMetricsOutputFormatter == null)
            {
                _defaultMetricsOutputFormatter = formatter;
            }

            _metricsOutputFormatters.TryAdd(formatter);
        });

        /// <inheritdoc />
        public IEnvOutputFormattingBuilder OutputEnvInfo => new EnvOutputFormattingBuilder(this, formatter =>
        {
            if (_defauEnvOutputFormatter == null)
            {
                _defauEnvOutputFormatter = formatter;
            }

            _envFormatters.TryAdd(formatter);
        });

        /// <inheritdoc />
        public IMetricsConfigurationBuilder Configuration
        {
            get
            {
                return new MetricsConfigurationBuilder(
                    this,
                    _options,
                    options =>
                    {
                        _options = options;
                    });
            }
        }

        /// <inheritdoc />
        public IMetricsReservoirSamplingBuilder SampleWith
        {
            get
            {
                return new MetricsReservoirSamplingBuilder(
                    this,
                    reservoir =>
                    {
                        _defaultSamplingReservoir = reservoir;
                    });
            }
        }

        /// <inheritdoc />
        public IMetricsRoot Build()
        {
            IMetricsRegistry registry = new NullMetricsRegistry();

            if (_options == null)
            {
                _options = new MetricsOptions();
            }

            if (_options.Enabled)
            {
                registry = new DefaultMetricsRegistry(_options.DefaultContextLabel, _clock, ContextRegistry);
            }

            if (_metricsOutputFormatters.Count == 0)
            {
                _metricsOutputFormatters.Add(new MetricsTextOutputFormatter());
            }

            if (_envFormatters.Count == 0)
            {
                _envFormatters.Add(new EnvInfoTextOutputFormatter());
            }

            var builderFactory = new DefaultMetricsBuilderFactory(_defaultSamplingReservoir);
            var measure = new DefaultMeasureMetricsProvider(registry, builderFactory, _clock);
            var provider = new DefaultMetricsProvider(registry, builderFactory, _clock);
            var snapshot = new DefaultMetricValuesProvider(_metricsFilter, registry);
            var manage = new DefaultMetricsManager(registry);
            var metrics = new DefaultMetrics(_clock, _metricsFilter, measure, builderFactory, provider, snapshot, manage);
            var defaultMetricsOutputFormatter = _defaultMetricsOutputFormatter ?? _metricsOutputFormatters.FirstOrDefault();
            var defaultEnvOutputFormatter = _defauEnvOutputFormatter ?? _envFormatters.FirstOrDefault();

            return new MetricsRoot(metrics, _options, _metricsOutputFormatters, _envFormatters, defaultMetricsOutputFormatter, defaultEnvOutputFormatter, _environmentInfoProvider);

            IMetricContextRegistry ContextRegistry(string context) => new DefaultMetricContextRegistry(context, new GlobalMetricTags(_options.GlobalTags));
        }
    }
}
