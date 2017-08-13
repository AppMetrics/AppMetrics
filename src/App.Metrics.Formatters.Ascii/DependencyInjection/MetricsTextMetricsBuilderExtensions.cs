// <copyright file="MetricsTextMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters.Ascii;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extensions methods for configuring App Metrics via an <see cref="IMetricsBuilder" />.
    /// </summary>
    public static class MetricsTextMetricsBuilderExtensions
    {
        /// <summary>
        ///     Adds configuration of <see cref="MetricsTextOptions" /> for the application.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" />.</param>
        /// <param name="setupAction">The <see cref="MetricsTextOptions" /> which need to be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IMetricsBuilder AddTextOptions(
            this IMetricsBuilder builder,
            Action<MetricsTextOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            builder.Services.Configure(setupAction);

            return builder;
        }
    }
}