// <copyright file="MetricsCoreMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class MetricsCoreMetricsBuilderExtensions
    {
        /// <summary>
        /// Registers an action to configure <see cref="MetricsOptions"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsBuilder"/>.</param>
        /// <param name="setupAction">An <see cref="Action{MetricsOptions}"/>.</param>
        /// <returns>The <see cref="IMetricsBuilder"/> instance.</returns>
        public static IMetricsBuilder AddMetricsOptions(
            this IMetricsBuilder builder,
            Action<MetricsOptions> setupAction)
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