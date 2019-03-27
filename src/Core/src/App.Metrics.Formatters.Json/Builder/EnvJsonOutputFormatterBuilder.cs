// <copyright file="EnvJsonOutputFormatterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.Json;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring environment information JSON output formatting using an
    ///     <see cref="IMetricsBuilder" />.
    /// </summary>
    public static class EnvJsonOutputFormatterBuilder
    {
        /// <summary>
        ///     Add the <see cref="EnvInfoJsonOutputFormatter" /> allowing metric values to optionally be reported as JSON.
        /// </summary>
        /// <param name="envFormattingBuilder">
        ///     The <see cref="IEnvOutputFormattingBuilder" /> used to configure JSON formatting
        ///     options.
        /// </param>
        /// <param name="setupAction">The JSON formatting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder AsJson(
            this IEnvOutputFormattingBuilder envFormattingBuilder,
            Action<MetricsJsonOptions> setupAction = null)
        {
            if (envFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(envFormattingBuilder));
            }

            var options = new MetricsJsonOptions();

            setupAction?.Invoke(options);

            var formatter = new EnvInfoJsonOutputFormatter(options.SerializerSettings);

            return envFormattingBuilder.Using(formatter);
        }
    }
}
