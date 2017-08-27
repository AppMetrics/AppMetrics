// <copyright file="EnvOutputFormattingBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring environment information output formatting using an <see cref="IMetricsBuilder" />.
    /// </summary>
    public class EnvOutputFormattingBuilder
    {
        private readonly IMetricsBuilder _metricsBuilder;
        private readonly Action<IEnvOutputFormatter> _formatter;

        internal EnvOutputFormattingBuilder(
            IMetricsBuilder metricsBuilder,
            Action<IEnvOutputFormatter> formatter)
        {
            _metricsBuilder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IEnvOutputFormatter"/> as one of the available formatters when reporting environment information.
        ///     </para>
        ///     <para>
        ///         Mulitple formatters can be used, in which case the <see cref="IMetricsRoot.DefaultOutputEnvFormatter"/> will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        /// <param name="formatter">An <see cref="IEnvOutputFormatter"/> instance used to format environment information when reporting.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder Using(IEnvOutputFormatter formatter)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            _formatter(formatter);

            return _metricsBuilder;
        }

        /// <summary>
        ///     <para>
        ///         Uses the specifed <see cref="IEnvOutputFormatter"/> as one of the available formatters when reporting environment information.
        ///     </para>
        ///     <para>
        ///         Mulitple formatters can be used, in which case the <see cref="IMetricsRoot.DefaultOutputEnvFormatter"/> will be set to the first configured formatter.
        ///     </para>
        /// </summary>
        /// <typeparam name="TEvnOutputFormatter">An <see cref="IEnvOutputFormatter"/> type used to format environment information when reporting.</typeparam>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public IMetricsBuilder Using<TEvnOutputFormatter>()
            where TEvnOutputFormatter : IEnvOutputFormatter, new()
        {
            _formatter(new TEvnOutputFormatter());

            return _metricsBuilder;
        }
    }
}
