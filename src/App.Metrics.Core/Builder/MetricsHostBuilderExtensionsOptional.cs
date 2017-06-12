// <copyright file="MetricsHostBuilderExtensionsOptional.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Configuration;
using App.Metrics.Core.DependencyInjection.Internal;
using App.Metrics.Core.Internal.NoOp;
using App.Metrics.Core.Reporting;
using App.Metrics.Core.ReservoirSampling;
using App.Metrics.Filters;
using App.Metrics.Reporting;
using App.Metrics.ReservoirSampling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class MetricsHostBuilderExtensionsOptional
    {
        public static IMetricsHostBuilder AddClockType<T>(this IMetricsHostBuilder builder)
            where T : class, IClock, new()
        {
            builder.Services.Replace(ServiceDescriptor.Singleton<IClock>(new T()));

            return builder;
        }

        /// <summary>
        ///     Adds the default reservoir which will be applied to all metrics using sampling that do not have an
        ///     <see cref="IReservoir" /> set explicitly.
        /// </summary>
        /// <param name="builder">The metrics host builder.</param>
        /// <param name="reservoirBuilder">The reservoir builder to use as the default reservoir for sampling.</param>
        /// <returns>The same instance of the metrics host builder.</returns>
        public static IMetricsHostBuilder AddDefaultReservoir(this IMetricsHostBuilder builder, Func<IReservoir> reservoirBuilder)
        {
            builder.Services.Remove(ServiceDescriptor.Singleton(new DefaultSamplingReservoirProvider(reservoirBuilder)));
            return builder;
        }

        public static IMetricsHostBuilder AddGlobalFilter(this IMetricsHostBuilder builder, IFilterMetrics filter)
        {
            builder.Services.Replace(ServiceDescriptor.Singleton(filter));

            return builder;
        }

        public static IMetricsHostBuilder AddReporting(this IMetricsHostBuilder builder, Action<IReportFactory> setupAction)
        {
            builder.Services.TryAddSingleton<AppMetricsReportingMarkerService, AppMetricsReportingMarkerService>();

            builder.Services.Replace(
                ServiceDescriptor.Singleton<IReportFactory>(
                    provider =>
                    {
                        var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                        var metrics = provider.GetRequiredService<IMetrics>();
                        var options = provider.GetRequiredService<AppMetricsOptions>();

                        if (!options.ReportingEnabled || setupAction == null)
                        {
                            return new NoOpReportFactory();
                        }

                        var factory = new ReportFactory(metrics, loggerFactory);
                        setupAction.Invoke(factory);
                        return factory;
                    }));

            return builder;
        }
    }
}