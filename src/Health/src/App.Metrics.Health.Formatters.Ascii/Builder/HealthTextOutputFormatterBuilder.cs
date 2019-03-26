// <copyright file="HealthTextOutputFormatterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health.Formatters;
using App.Metrics.Health.Formatters.Ascii;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring metric plain text output formatting using an
    ///     <see cref="IHealthBuilder" />.
    /// </summary>
    public static class HealthTextOutputFormatterBuilder
    {
        /// <summary>
        ///     Add the <see cref="IHealthOutputFormatter" /> allowing health checks to optionally be reported as plain text.
        /// </summary>
        /// <param name="healthFormattingBuilder">
        ///     The <see cref="IHealthOutputFormattingBuilder" /> used to configure formatting
        ///     options.
        /// </param>
        /// <param name="setupAction">The plain text formatting options to use.</param>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics Health.
        /// </returns>
        public static IHealthBuilder AsPlainText(
            this IHealthOutputFormattingBuilder healthFormattingBuilder,
            Action<HealthTextOptions> setupAction = null)
        {
            if (healthFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(healthFormattingBuilder));
            }

            var options = new HealthTextOptions();

            setupAction?.Invoke(options);

            var formatter = new HealthStatusTextOutputFormatter();

            return healthFormattingBuilder.Using(formatter);
        }
    }
}
