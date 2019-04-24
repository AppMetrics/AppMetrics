// <copyright file="HealthJsonOutputFormatterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health.Formatters.Json;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring metric JSON output formatting using an
    ///     <see cref="IHealthBuilder" />.
    /// </summary>
    public static class HealthJsonOutputFormatterBuilder
    {
        /// <summary>
        ///     Add the <see cref="HealthStatusJsonOutputFormatter" /> allowing health check results to optionally be reported as JSON.
        /// </summary>
        /// <param name="metricFormattingBuilder">
        ///     The <see cref="IHealthOutputFormattingBuilder" /> used to configure JSON formatting
        ///     options.
        /// </param>
        /// <param name="setupAction">The JSON formatting options to use.</param>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics Health.
        /// </returns>
        public static IHealthBuilder AsJson(
            this IHealthOutputFormattingBuilder metricFormattingBuilder,
            Action<HealthJsonOptions> setupAction = null)
        {
            if (metricFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(metricFormattingBuilder));
            }

            var options = new HealthJsonOptions();

            setupAction?.Invoke(options);

            var formatter = new HealthStatusJsonOutputFormatter(options.SerializerSettings);

            return metricFormattingBuilder.Using(formatter);
        }
    }
}
