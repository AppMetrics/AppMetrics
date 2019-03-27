// <copyright file="AppMetricsServicesHelper.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using App.Metrics.Internal.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Extensions.DependencyInjection.Internal
{
    [ExcludeFromCodeCoverage]
    public static class AppMetricsServicesHelper
    {
        private static readonly string InvalidOperationExceptionMessage =
            "Unable to find the App Metrics required services. Please add all required services by calling IServiceCollection.AddMetrics(); or equivalent";

        /// <summary>
        ///     Throws InvalidOperationException when MetricsMarkerService is not present
        ///     in the list of services.
        /// </summary>
        /// <param name="services">The list of services.</param>
        public static void ThrowIfMetricsNotRegistered(IServiceProvider services)
        {
            if (services.GetService(typeof(AppMetricsMarkerService)) == null)
            {
                throw new InvalidOperationException(InvalidOperationExceptionMessage);
            }
        }

        /// <summary>
        ///     Throws InvalidOperationException when MetricsMarkerService is not present
        ///     in the list of services.
        /// </summary>
        /// <param name="services">The list of services.</param>
        public static void ThrowIfMetricsNotRegistered(IServiceCollection services)
        {
            if (services.All(s => s.ServiceType != typeof(AppMetricsMarkerService)))
            {
                throw new InvalidOperationException(InvalidOperationExceptionMessage);
            }
        }
    }
}