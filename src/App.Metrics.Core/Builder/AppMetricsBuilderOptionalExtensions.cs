// <copyright file="AppMetricsBuilderOptionalExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.ReservoirSampling;
using App.Metrics.Filters;
using App.Metrics.ReservoirSampling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsBuilderOptionalExtensions
    {
        public static IAppMetricsBuilder AddClockType<T>(this IAppMetricsBuilder builder)
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
        public static IAppMetricsBuilder AddDefaultReservoir(this IAppMetricsBuilder builder, Func<IReservoir> reservoirBuilder)
        {
            builder.Services.Remove(ServiceDescriptor.Singleton(new DefaultSamplingReservoirProvider(reservoirBuilder)));
            return builder;
        }

        public static IAppMetricsBuilder AddGlobalFilter(this IAppMetricsBuilder builder, IFilterMetrics filter)
        {
            builder.Services.Replace(ServiceDescriptor.Singleton(filter));

            return builder;
        }
    }
}