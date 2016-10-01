using System;
using System.Diagnostics;
using App.Metrics.Reporters;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics
{
    public sealed class MetricsConfig : IDisposable, IHideObjectMembers
    {
        //TODO: AH - Inject Logger
        private static readonly ILogger Log = new LoggerFactory().CreateLogger(typeof(MetricsConfig));
        public static readonly bool GloballyDisabledMetrics = ReadGloballyDisableMetricsSetting();


        private readonly MetricsContext _context;
        private readonly MetricsReports _reports;

        private SamplingType _defaultSamplingType = SamplingType.ExponentiallyDecaying;
        private Func<HealthStatus> _healthStatus;
        private bool _isDisabled = GloballyDisabledMetrics;

        public MetricsConfig(MetricsContext context)
        {
            _context = context;

            if (!GloballyDisabledMetrics)
            {
                _healthStatus = HealthChecks.GetStatus;
                _reports = new MetricsReports(_context.DataProvider, _healthStatus);

                _context.Advanced.ContextDisabled += (s, e) =>
                {
                    _isDisabled = true;
                    DisableAllReports();
                };
            }
        }

        /// <summary>
        ///     Gets the currently configured default sampling type to use for histogram sampling.
        /// </summary>
        public SamplingType DefaultSamplingType
        {
            get
            {
                Debug.Assert(_defaultSamplingType != SamplingType.Default);
                return _defaultSamplingType;
            }
        }

        public void Dispose()
        {
            _reports.Dispose();
        }

        /// <summary>
        ///     This method is used for customizing the metrics configuration.
        ///     The <paramref name="extension" /> will be called with the current MetricsContext and HealthStatus provider.
        /// </summary>
        /// <remarks>
        ///     In general you don't need to call this method directly.
        /// </remarks>
        /// <param name="extension">Action to apply extra configuration.</param>
        /// <returns>Chain-able configuration object.</returns>
        public MetricsConfig WithConfigExtension(Action<MetricsContext, Func<HealthStatus>> extension)
        {
            return WithConfigExtension((m, h) =>
            {
                extension(m, h);
                return this;
            });
        }

        /// <summary>
        ///     This method is used for customizing the metrics configuration.
        ///     The <paramref name="extension" /> will be called with the current MetricsContext and HealthStatus provider.
        /// </summary>
        /// <remarks>
        ///     In general you don't need to call this method directly.
        /// </remarks>
        /// <param name="extension">Action to apply extra configuration.</param>
        /// <returns>The result of calling the extension.</returns>
        public T WithConfigExtension<T>(Func<MetricsContext, Func<HealthStatus>, T> extension)
        {
            return extension(_context, _healthStatus);
        }

        /// <summary>
        ///     Configure the default sampling type to use for histograms.
        /// </summary>
        /// <param name="type">Type of sampling to use.</param>
        /// <returns>Chain-able configuration object.</returns>
        public MetricsConfig WithDefaultSamplingType(SamplingType type)
        {
            if (type == SamplingType.Default)
            {
                throw new ArgumentException("Sampling type other than default must be specified", nameof(type));
            }
            _defaultSamplingType = type;
            return this;
        }

        /// <summary>
        ///     Error handler for the metrics library. If a handler is registered any error will be passed to the handler.
        ///     By default unhandled errors are logged, printed to console if Environment.UserInteractive is true, and logged with
        ///     Trace.TracError.
        /// </summary>
        /// <param name="errorHandler">Action with will be executed with the exception.</param>
        /// <param name="clearExistingHandlers">Is set to true, remove any existing handler.</param>
        /// <returns>Chain able configuration object.</returns>
        public MetricsConfig WithErrorHandler(Action<Exception> errorHandler, bool clearExistingHandlers = false)
        {
            if (clearExistingHandlers)
            {
                MetricsErrorHandler.Handler.ClearHandlers();
            }

            if (!_isDisabled)
            {
                MetricsErrorHandler.Handler.AddHandler(errorHandler);
            }

            return this;
        }

        /// <summary>
        ///     Error handler for the metrics library. If a handler is registered any error will be passed to the handler.
        ///     By default unhandled errors are logged, printed to console if Environment.UserInteractive is true, and logged with
        ///     Trace.TracError.
        /// </summary>
        /// <param name="errorHandler">Action with will be executed with the exception and a specific message.</param>
        /// <param name="clearExistingHandlers">Is set to true, remove any existing handler.</param>
        /// <returns>Chain able configuration object.</returns>
        public MetricsConfig WithErrorHandler(Action<Exception, string> errorHandler, bool clearExistingHandlers = false)
        {
            if (clearExistingHandlers)
            {
                MetricsErrorHandler.Handler.ClearHandlers();
            }

            if (!_isDisabled)
            {
                MetricsErrorHandler.Handler.AddHandler(errorHandler);
            }

            return this;
        }

        /// <summary>
        ///     Configure Metrics library to use a custom health status reporter. By default HealthChecks.GetStatus() is used.
        /// </summary>
        /// <param name="healthStatus">Function that provides the current health status.</param>
        /// <returns>Chain-able configuration object.</returns>
        public MetricsConfig WithHealthStatus(Func<HealthStatus> healthStatus)
        {
            if (!_isDisabled)
            {
                _healthStatus = healthStatus;
            }
            return this;
        }

        public MetricsConfig WithInternalMetrics()
        {
            Metric.EnableInternalMetrics();
            return this;
        }

        /// <summary>
        ///     Configure the way metrics are reported
        /// </summary>
        /// <param name="reportsConfig">Reports configuration action</param>
        /// <returns>Chain-able configuration object.</returns>
        public MetricsConfig WithReporting(Action<MetricsReports> reportsConfig)
        {
            if (!_isDisabled)
            {
                reportsConfig(_reports);
            }

            return this;
        }

        internal void ApplySettingsFromConfigFile()
        {
            if (!GloballyDisabledMetrics)
            {
            }
        }

        private static bool ReadGloballyDisableMetricsSetting()
        {
            try
            {
                //TODO: Inject IOptions<> to allow metrics to be disabled
                //var isDisabled = ConfigurationManager.AppSettings["Metrics.CompletelyDisableMetrics"];
                return false;
            }
            catch (Exception x)
            {
                MetricsErrorHandler.Handle(x, "Invalid Metrics Configuration: Metrics.CompletelyDisableMetrics must be set to true or false");
                return false;
            }
        }

        private void DisableAllReports()
        {
            this._reports.StopAndClearAllReports();
        }
    }
}