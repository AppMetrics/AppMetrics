// <copyright file="MetricsCoreMetricsCoreBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.Configuration;
using App.Metrics.Filters;
using App.Metrics.ReservoirSampling;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extensions for configuring App Metrics using an <see cref="IMetricsCoreBuilder" />.
    /// </summary>
    public static class MetricsCoreMetricsCoreBuilderExtensions
    {
        public static IMetricsCoreBuilder AddClockType<T>(this IMetricsCoreBuilder builder)
            where T : class, IClock, new()
        {
            var destriptor = ServiceDescriptor.Singleton<IClock>(new T());

            builder.Services.Replace(destriptor);

            return builder;
        }

        /// <summary>
        ///     Adds the default reservoir which will be applied to all metrics using sampling that do not have an
        ///     <see cref="IReservoir" /> set explicitly.
        /// </summary>
        /// <param name="builder">The metrics host builder.</param>
        /// <param name="reservoirBuilder">The reservoir builder to use as the default reservoir for sampling.</param>
        /// <returns>The same instance of the metrics host builder.</returns>
        public static IMetricsCoreBuilder AddDefaultReservoir(this IMetricsCoreBuilder builder, Func<IReservoir> reservoirBuilder)
        {
            var descriptor = ServiceDescriptor.Singleton(new DefaultSamplingReservoirProvider(reservoirBuilder));

            builder.Services.Replace(descriptor);

            return builder;
        }

        public static IMetricsCoreBuilder AddGlobalFilter(this IMetricsCoreBuilder builder, IFilterMetrics filter)
        {
            var descripter = ServiceDescriptor.Singleton(filter);

            builder.Services.Replace(descripter);

            return builder;
        }

        /// <summary>
        ///     Registers an action to configure <see cref="MetricsOptions" />.
        /// </summary>
        /// <param name="builder">The <see cref="IMetricsCoreBuilder" />.</param>
        /// <param name="setupAction">An <see cref="Action{MetricsOptions}" />.</param>
        /// <returns>The <see cref="IMetricsCoreBuilder" /> instance.</returns>
        public static IMetricsCoreBuilder AddMetricsOptions(
            this IMetricsCoreBuilder builder,
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