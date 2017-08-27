// <copyright file="EnvTextOutputFormatterBuider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.Ascii;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring environment information plain text output formatting using an
    ///     <see cref="IMetricsBuilder" />.
    /// </summary>
    public static class EnvTextOutputFormatterBuider
    {
        /// <summary>
        ///     Add the <see cref="EnvInfoTextOutputFormatter" /> allowing environment information to optionally be reported as plain text.
        /// </summary>
        /// <param name="envFormattingBuilder">
        ///     The <see cref="EnvOutputFormattingBuilder" /> used to configuring formatting
        ///     options.
        /// </param>
        /// <param name="setupAction">The plain text formatting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure the App Metrics.
        /// </returns>
        public static IMetricsBuilder AsPlainText(
            this EnvOutputFormattingBuilder envFormattingBuilder,
            Action<MetricsTextOptions> setupAction = null)
        {
            if (envFormattingBuilder == null)
            {
                throw new ArgumentNullException(nameof(envFormattingBuilder));
            }

            var options = new MetricsTextOptions();

            setupAction?.Invoke(options);

            var formatter = new EnvInfoTextOutputFormatter(options);

            return envFormattingBuilder.Using(formatter);
        }
    }
}