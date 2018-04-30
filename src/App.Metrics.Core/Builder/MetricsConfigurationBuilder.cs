// <copyright file="MetricsConfigurationBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Internal;
using App.Metrics.Internal.Extensions;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring the <see cref="MetricsOptions" />.
    /// </summary>
    public class MetricsConfigurationBuilder : IMetricsConfigurationBuilder
    {
        private readonly Action<MetricsOptions> _setupAction;
        private MetricsOptions _options;

        internal MetricsConfigurationBuilder(
            IMetricsBuilder metricsBuilder,
            MetricsOptions currentOptions,
            Action<MetricsOptions> setupAction)
        {
            Builder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _setupAction = setupAction ?? throw new ArgumentNullException(nameof(setupAction));
            _options = currentOptions ?? new MetricsOptions();
        }

        /// <inheritdoc />
        public IMetricsBuilder Builder { get; }

        /// <inheritdoc />
        public IMetricsBuilder Configure(MetricsOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _setupAction(options);

            _options = options;

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder Configure(IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            var mergedOptions = new KeyValuePairMetricsOptions(_options, optionValues).AsOptions();

            _setupAction(mergedOptions);

            _options = mergedOptions;

            return Builder;
        }

        /// <inheritdoc />
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

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder Configure(Action<MetricsOptions> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            setupAction(_options);

            _setupAction(_options);

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder Extend(IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            var mergedOptions = new KeyValuePairMetricsOptions(_options, optionValues).AsOptions(true);

            _setupAction(mergedOptions);

            _options = mergedOptions;

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder Extend(MetricsOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var optionsValuesToExtend = options.ToKeyValue();
            var extendedOptions = new KeyValuePairMetricsOptions(_options, optionsValuesToExtend).AsOptions(true);

            _setupAction(extendedOptions);

            _options = extendedOptions;

            return Builder;
        }
    }
}