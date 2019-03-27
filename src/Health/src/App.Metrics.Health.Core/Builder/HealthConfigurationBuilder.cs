// <copyright file="HealthConfigurationBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Health.Internal;
using App.Metrics.Health.Internal.Extensions;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring the <see cref="HealthOptions" />.
    /// </summary>
    public class HealthConfigurationBuilder : IHealthConfigurationBuilder
    {
        private readonly Action<HealthOptions> _setupAction;
        private HealthOptions _options;

        internal HealthConfigurationBuilder(
            IHealthBuilder healthBuilder,
            HealthOptions currentOptions,
            Action<HealthOptions> setupAction)
        {
            Builder = healthBuilder ?? throw new ArgumentNullException(nameof(healthBuilder));
            _setupAction = setupAction ?? throw new ArgumentNullException(nameof(setupAction));
            _options = currentOptions ?? new HealthOptions();
        }

        /// <inheritdoc />
        public IHealthBuilder Builder { get; }

        /// <inheritdoc />
        public IHealthBuilder Configure(HealthOptions options)
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
        public IHealthBuilder Configure(IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            var mergedOptions = new KeyValuePairHealthOptions(_options, optionValues).AsOptions();

            _setupAction(mergedOptions);

            _options = mergedOptions;

            return Builder;
        }

        /// <inheritdoc />
        public IHealthBuilder Configure(HealthOptions options, IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            _setupAction(new KeyValuePairHealthOptions(options, optionValues).AsOptions());

            _options = options;

            return Builder;
        }

        /// <inheritdoc />
        public IHealthBuilder Configure(Action<HealthOptions> setupAction)
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
        public IHealthBuilder Extend(IEnumerable<KeyValuePair<string, string>> optionValues)
        {
            if (optionValues == null)
            {
                throw new ArgumentNullException(nameof(optionValues));
            }

            var mergedOptions = new KeyValuePairHealthOptions(_options, optionValues).AsOptions();

            _setupAction(mergedOptions);

            _options = mergedOptions;

            return Builder;
        }

        /// <inheritdoc />
        public IHealthBuilder Extend(HealthOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var optionsValuesToExtend = options.ToKeyValue();
            var extendedOptions = new KeyValuePairHealthOptions(_options, optionsValuesToExtend).AsOptions();

            _setupAction(extendedOptions);

            _options = extendedOptions;

            return Builder;
        }
    }
}