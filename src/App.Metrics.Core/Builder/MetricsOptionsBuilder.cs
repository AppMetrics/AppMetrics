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
    ///     Builder for configuring the <see cref="MetricsOptions"/>.
    /// </summary>
    public class MetricsOptionsBuilder
    {
        private readonly EnvironmentInfoProvider _environmentInfoProvider;
        private readonly IMetricsBuilder _metricsBuilder;
        private readonly Action<MetricsOptions> _setupAction;

        internal MetricsOptionsBuilder(
            IMetricsBuilder metricsBuilder,
            Action<MetricsOptions> options,
            EnvironmentInfoProvider environmentInfoProvider)
        {
            _environmentInfoProvider = environmentInfoProvider ?? throw new ArgumentNullException(nameof(environmentInfoProvider));
            _metricsBuilder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _setupAction = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="MetricsOptions"/> instance for App Metrics core configuration.
        ///     </para>
        /// </summary>
        /// <param name="options">An <see cref="MetricsOptions"/> instance used to configure core App Metrics options.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder Configure(MetricsOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _setupAction(options);

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed key value pairs to configure an <see cref="MetricsOptions"/> instance for App Metrics core configuration.
        ///     </para>
        ///     <para>
        ///         Keys match the <see cref="MetricsOptions"/>s property names.
        ///     </para>
        /// </summary>
        /// <param name="optionValues">Key value pairs for configuring App Metrics</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder Configure(IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            var options = new KeyValuePairMetricsOptions(optionValues).AsOptions();

            _setupAction(options);

            AddDefaultTags(options);

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed key value pairs to configure an <see cref="MetricsOptions"/> instance for App Metrics core configuration.
        ///     </para>
        ///     <para>
        ///         Keys match the <see cref="MetricsOptions"/>s property names. Any make key will override the <see cref="MetricsOptions"/> value configured.
        ///     </para>
        /// </summary>
        /// <param name="options">An <see cref="MetricsOptions"/> instance used to configure core App Metrics options.</param>
        /// <param name="optionValues">Key value pairs for configuring App Metrics</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder Configure(MetricsOptions options, IEnumerable<KeyValuePair<string, string>> optionValues)
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

            AddDefaultTags(options);

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed key value pairs to configure an <see cref="MetricsOptions"/> instance for App Metrics core configuration.
        ///     </para>
        ///     <para>
        ///         Keys match the <see cref="MetricsOptions"/>s property names. Any make key will override the <see cref="MetricsOptions"/> value configured.
        ///     </para>
        /// </summary>
        /// <param name="setupAction">An <see cref="MetricsOptions"/> setup action used to configure core App Metrics options.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder Configure(Action<MetricsOptions> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            var options = new MetricsOptions();

            setupAction(options);

            _setupAction(options);

            AddDefaultTags(options);

            return _metricsBuilder;
        }

        private void AddDefaultTags(MetricsOptions options)
        {
            if (!options.AddDefaultGlobalTags)
            {
                return;
            }

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
    }
}
