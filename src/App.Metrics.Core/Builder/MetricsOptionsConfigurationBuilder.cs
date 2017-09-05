// <copyright file="MetricsOptionsConfigurationBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring the <see cref="MetricsOptions" />.
    /// </summary>
    public class MetricsOptionsConfigurationBuilder
    {
        private readonly IMetricsBuilder _metricsBuilder;
        private readonly Action<MetricsOptions> _setupAction;
        private MetricsOptions _options;

        internal MetricsOptionsConfigurationBuilder(
            IMetricsBuilder metricsBuilder,
            MetricsOptions currentOptions,
            Action<MetricsOptions> setupAction)
        {
            _metricsBuilder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _setupAction = setupAction ?? throw new ArgumentNullException(nameof(setupAction));
            _options = currentOptions ?? new MetricsOptions();
        }

        public MetricsOptions Options => _options;

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="MetricsOptions" /> instance for App Metrics core configuration.
        ///     </para>
        /// </summary>
        /// <param name="options">An <see cref="MetricsOptions" /> instance used to configure core App Metrics options.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public IMetricsBuilder Configure(MetricsOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _setupAction(options);

            _options = options;

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
        public IMetricsBuilder Configure(IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            var mergedOptions = new KeyValuePairMetricsOptions(_options, optionValues).AsOptions();

            _setupAction(mergedOptions);

            _options = mergedOptions;

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

            _options = options;

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
        public IMetricsBuilder Configure(Action<MetricsOptions> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            setupAction(_options);

            _setupAction(_options);

            return _metricsBuilder;
        }
    }
}