// <copyright file="MetricsTextMetricsCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extensions methods for configuring App Metrics via an <see cref="IMetricsBuilder" />.
    /// </summary>
    public static class MetricsTextMetricsCoreBuilderExtensions
    {
        /// <summary>
        ///     Adds the default Text <see cref="IMetricsOutputFormatter" /> and <see cref="IEnvOutputFormatter" /> to
        ///     <see cref="MetricsOptions" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IMetricsCoreBuilder AddTextFormatters(this IMetricsCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            AddTextFormatterServices(builder.Services);

            return builder;
        }

        internal static void AddTextFormatterServices(IServiceCollection services)
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<MetricsOptions>, MetricsTextOptionsSetup>());
        }
    }
}
