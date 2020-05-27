// <copyright file="MetricsAspNetWebHostBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using App.Metrics.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Hosting
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for setting up App Metrics AspNet Core services in an <see cref="IHostBuilder" />.
    /// </summary>
    public static class MetricsAspNetHostBuilderExtensions
    {
        private static bool _metricsBuilt;

        public static IHostBuilder ConfigureMetricsWithDefaults(
            this IHostBuilder hostBuilder,
            Action<HostBuilderContext, IMetricsBuilder> configureMetrics)
        {
            if (_metricsBuilt)
            {
                throw new InvalidOperationException("MetricsBuilder allows creation only of a single instance of IMetrics");
            }

            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var metricsBuilder = AppMetrics.CreateDefaultBuilder();
                    configureMetrics(context, metricsBuilder);
                    metricsBuilder.Configuration.ReadFrom(context.Configuration);
                    services.AddMetrics(metricsBuilder);
                    _metricsBuilt = true;
                });
        }

        public static IHostBuilder ConfigureMetricsWithDefaults(this IHostBuilder hostBuilder, Action<IMetricsBuilder> configureMetrics)
        {
            if (_metricsBuilt)
            {
                throw new InvalidOperationException("MetricsBuilder allows creation only of a single instance of IMetrics");
            }

            hostBuilder.ConfigureMetricsWithDefaults(
                (context, builder) =>
                {
                    configureMetrics(builder);
                });

            return hostBuilder;
        }

        public static IHostBuilder ConfigureMetrics(this IHostBuilder hostBuilder, IMetricsRoot metrics)
        {
            if (_metricsBuilt)
            {
                throw new InvalidOperationException("MetricsBuilder allows creation only of a single instance of IMetrics");
            }

            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddMetrics(metrics);
                    _metricsBuilt = true;
                });
        }

        public static IHostBuilder ConfigureMetrics(
            this IHostBuilder hostBuilder,
            Action<HostBuilderContext, IMetricsBuilder> configureMetrics)
        {
            if (_metricsBuilt)
            {
                throw new InvalidOperationException("MetricsBuilder allows creation only of a single instance of IMetrics");
            }

            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddMetrics(
                        builder =>
                        {
                            configureMetrics(context, builder);
                            builder.Configuration.ReadFrom(context.Configuration);
                            _metricsBuilt = true;
                        });
                });
        }

        public static IHostBuilder ConfigureMetrics(this IHostBuilder hostBuilder, Action<IMetricsBuilder> configureMetrics)
        {
            if (_metricsBuilt)
            {
                throw new InvalidOperationException("MetricsBuilder allows creation only of a single instance of IMetrics");
            }

            hostBuilder.ConfigureMetrics(
                (context, builder) =>
                {
                    configureMetrics(builder);
                });

            return hostBuilder;
        }

        public static IHostBuilder ConfigureMetrics(this IHostBuilder hostBuilder)
        {
            if (_metricsBuilt)
            {
                return hostBuilder;
            }

            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    if (!_metricsBuilt)
                    {
                        var builder = AppMetrics.CreateDefaultBuilder()
                            .Configuration.ReadFrom(context.Configuration);
                        services.AddMetrics(builder);
                        _metricsBuilt = true;
                    }
                });
        }
    }
}