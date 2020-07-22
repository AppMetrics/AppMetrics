// <copyright file="MetricsBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using App.Metrics.Builder;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;
using App.Metrics.Internal.NoOp;
using App.Metrics.Registry;
using App.Metrics.Reporting;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public class MetricsBuilder : IMetricsBuilder
    {
        private readonly EnvFormatterCollection _envFormatters = new EnvFormatterCollection();
        private readonly EnvironmentInfoProvider _environmentInfoProvider = new EnvironmentInfoProvider();
        private readonly MetricsFormatterCollection _metricsOutputFormatters = new MetricsFormatterCollection();
        private readonly MetricsReporterCollection _reporters = new MetricsReporterCollection();
        private IClock _clock = new StopwatchClock();
        private IEnvOutputFormatter _defauEnvOutputFormatter;
        private IMetricsOutputFormatter _defaultMetricsOutputFormatter;

        private DefaultSamplingReservoirProvider _defaultSamplingReservoir =
            new DefaultSamplingReservoirProvider(() => new DefaultForwardDecayingReservoir());

        private IFilterMetrics _metricsFilter = new NullMetricsFilter();
        private IRunMetricsReports _metricsReportRunner = new NoOpMetricsReportRunner();
        private MetricsOptions _options;
        private MetricFields _metricFields;

        /// <inheritdoc />
        public IMetricsConfigurationBuilder Configuration
        {
            get
            {
                return new MetricsConfigurationBuilder(
                    this,
                    _options,
                    options => { _options = options; });
            }
        }

        /// <inheritdoc />
        public IMetricFieldsBuilder MetricFields
        {
            get
            {
                return new MetricFieldsBuilder(
                    this,
                    _metricFields,
                    metricFields => { _metricFields = metricFields; });
            }
        }

        /// <inheritdoc />
        public IMetricsFilterBuilder Filter
        {
            get
            {
                return new MetricsFilterBuilder(
                    this,
                    metricsFilter => { _metricsFilter = metricsFilter; });
            }
        }

        /// <inheritdoc />
        public IEnvOutputFormattingBuilder OutputEnvInfo => new EnvOutputFormattingBuilder(
            this,
            formatter =>
            {
                if (_defauEnvOutputFormatter == null)
                {
                    _defauEnvOutputFormatter = formatter;
                }

                _envFormatters.TryAdd(formatter);
            });

        /// <inheritdoc />
        public IMetricsOutputFormattingBuilder OutputMetrics => new MetricsOutputFormattingBuilder(
            this,
            (replaceExisting, formatter) =>
            {
                if (_defaultMetricsOutputFormatter == null)
                {
                    _defaultMetricsOutputFormatter = formatter;
                }

                if (replaceExisting)
                {
                    _metricsOutputFormatters.TryAdd(formatter);
                }
                else
                {
                    if (_metricsOutputFormatters.GetType(formatter.GetType()) == null)
                    {
                        _metricsOutputFormatters.Add(formatter);
                    }
                }
            });

        /// <inheritdoc />
        public IMetricsReportingBuilder Report => new MetricsReportingBuilder(
            this,
            _metricsOutputFormatters,
            reporter => { _reporters.TryAdd(reporter); });

        /// <inheritdoc />
        public IMetricsReservoirSamplingBuilder SampleWith
        {
            get
            {
                return new MetricsReservoirSamplingBuilder(
                    this,
                    reservoir => { _defaultSamplingReservoir = reservoir; });
            }
        }

        /// <inheritdoc />
        public IMetricsClockBuilder TimeWith
        {
            get
            {
                return new MetricsClockBuilder(
                    this,
                    clock => { _clock = clock; });
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

            if (_metricFields == null)
            {
                _metricFields = new MetricFields();
            }

            if (_options.Enabled)
            {
                registry = new DefaultMetricsRegistry(_options.DefaultContextLabel, _clock, ContextRegistry);
            }

            if (_metricsOutputFormatters.Count == 0)
            {
                _metricsOutputFormatters.Add(new MetricsTextOutputFormatter(_metricFields));
            }
            else
            {
                foreach (var metricsOutputFormatter in _metricsOutputFormatters)
                {
                    if (metricsOutputFormatter.MetricFields == null)
                    {
                        metricsOutputFormatter.MetricFields = _metricFields;
                    }
                }
            }

            if (_reporters != null && _reporters.Any())
            {
                foreach (var reporter in _reporters)
                {
                    if (reporter.Formatter != null && reporter.Formatter.MetricFields == null)
                    {
                        reporter.Formatter.MetricFields = _metricFields;
                    }
                }
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

            if (CanReport())
            {
                _metricsReportRunner = new DefaultMetricsReportRunner(metrics, _reporters);
            }

            var metricsRoot = new MetricsRoot(
                metrics,
                _options,
                _metricsOutputFormatters,
                _envFormatters,
                defaultMetricsOutputFormatter,
                defaultEnvOutputFormatter,
                _environmentInfoProvider,
                _reporters,
                _metricsReportRunner);
            
            Metrics.SetInstance(metricsRoot);

            return Metrics.Instance;

            IMetricContextRegistry ContextRegistry(string context) =>
                new DefaultMetricContextRegistry(
                    context,
                    new GlobalMetricTags(_options.GlobalTags),
                    new ContextualMetricTagProviders(_options.ContextualTags));
        }

        public bool CanReport() { return _options.Enabled && _options.ReportingEnabled && _reporters.Any(); }
    }
}