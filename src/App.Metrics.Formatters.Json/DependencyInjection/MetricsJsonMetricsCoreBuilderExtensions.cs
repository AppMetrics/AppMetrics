// <copyright file="MetricsJsonMetricsCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Json.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extensions methods for configuring App Metrics via an <see cref="IMetricsBuilder" />.
    /// </summary>
    public static class MetricsJsonMetricsCoreBuilderExtensions
    {
        /// <summary>
        ///     Adds the default JSON <see cref="IMetricsOutputFormatter" /> and <see cref="IEnvOutputFormatter" /> to
        ///     <see cref="MetricsOptions" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IMetricsCoreBuilder AddJsonFormatters(this IMetricsCoreBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            AddJsonFormatterServices(builder.Services);

            return builder;
        }

        internal static void AddJsonFormatterServices(IServiceCollection services)
        {
            var jsonOptionsSetupServiceDescriptor = ServiceDescriptor.Transient<IConfigureOptions<MetricsOptions>, MetricsJsonOptionsSetup>();
            services.TryAddEnumerable(jsonOptionsSetupServiceDescriptor);
        }
    }
}