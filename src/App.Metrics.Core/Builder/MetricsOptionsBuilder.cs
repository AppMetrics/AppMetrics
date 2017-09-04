// <copyright file="MetricsOptionsBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Infrastructure;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring the <see cref="MetricsOptions" />.
    /// </summary>
    public class MetricsOptionsBuilder
    {
        private readonly EnvironmentInfoProvider _environmentInfoProvider;
        private readonly IMetricsBuilder _metricsBuilder;
        private readonly Action<MetricsOptions> _setupAction;

        internal MetricsOptionsBuilder(
            IMetricsBuilder metricsBuilder,
            Action<MetricsOptions> setupAction,
            EnvironmentInfoProvider environmentInfoProvider)
        {
            _environmentInfoProvider = environmentInfoProvider ?? throw new ArgumentNullException(nameof(environmentInfoProvider));
            _metricsBuilder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _setupAction = setupAction ?? throw new ArgumentNullException(nameof(setupAction));
        }

        public MetricsOptions Options { get; private set; } = new MetricsOptions();

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="MetricsOptions" /> instance for App Metrics core configuration.
        ///     </para>
        /// </summary>
        /// <param name="options">An <see cref="MetricsOptions" /> instance used to configure core App Metrics options.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public IMetricsBuilder Use(MetricsOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _setupAction(options);

            RefreshOptions(options);

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed key value pairs to configure an <see cref="MetricsOptions" /> instance for App Metrics core
        ///         configuration.
        ///     </para>
        ///     <para>
        ///         Keys match the <see cref="MetricsOptions" />s property names.
        ///     </para>
        /// </summary>
        /// <param name="optionValues">Key value pairs for configuring App Metrics</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public IMetricsBuilder Use(IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            var mergedOptions = new KeyValuePairMetricsOptions(Options, optionValues).AsOptions();

            _setupAction(mergedOptions);

            RefreshOptions(mergedOptions);

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed key value pairs to configure an <see cref="MetricsOptions" /> instance for App Metrics core
        ///         configuration.
        ///     </para>
        ///     <para>
        ///         Keys match the <see cref="MetricsOptions" />s property names. Any make key will override the
        ///         <see cref="MetricsOptions" /> value configured.
        ///     </para>
        /// </summary>
        /// <param name="options">An <see cref="MetricsOptions" /> instance used to configure core App Metrics options.</param>
        /// <param name="optionValues">Key value pairs for configuring App Metrics</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public IMetricsBuilder Use(MetricsOptions options, IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            _setupAction(new KeyValuePairMetricsOptions(options, optionValues).AsOptions());

            RefreshOptions(options);

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed key value pairs to configure an <see cref="MetricsOptions" /> instance for App Metrics core
        ///         configuration.
        ///     </para>
        ///     <para>
        ///         Keys match the <see cref="MetricsOptions" />s property names. Any make key will override the
        ///         <see cref="MetricsOptions" /> value configured.
        ///     </para>
        /// </summary>
        /// <param name="setupAction">An <see cref="MetricsOptions" /> setup action used to configure core App Metrics options.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public IMetricsBuilder Use(Action<MetricsOptions> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            setupAction(Options);

            _setupAction(Options);

            RefreshOptions(Options);

            return _metricsBuilder;
        }

        private void RefreshOptions(MetricsOptions options)
        {
            if (options.AddDefaultGlobalTags)
            {
                var environmentInfo = _environmentInfoProvider.Build();

                if (!options.GlobalTags.ContainsKey("app"))
                {
                    options.GlobalTags.Add("app", environmentInfo.EntryAssemblyName);
                }

                if (!options.GlobalTags.ContainsKey("server"))
                {
                    options.GlobalTags.Add("server", environmentInfo.MachineName);
                }

                if (!options.GlobalTags.ContainsKey("env"))
                {
#if DEBUG
                    options.GlobalTags.Add("env", "debug");
#else
                    options.GlobalTags.Add("env", "release");
#endif
                }
            }

            Options = options;
        }
    }
}