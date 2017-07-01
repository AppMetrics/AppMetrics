// <copyright file="AppMetricsMiddlewareServicesHelper.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Core.DependencyInjection.Internal
{
    internal static class AppMetricsMiddlewareServicesHelper
    {
        /// <summary>
        ///     Throws InvalidOperationException when MetricsMarkerService is not present
        ///     in the list of services.
        /// </summary>
        /// <param name="services">The list of services.</param>
        public static void ThrowIfMetricsNotRegistered(IServiceProvider services)
        {
            if (services.GetService(typeof(AppMetricsMiddlewareMarkerService)) == null)
            {
                throw new InvalidOperationException(
                    "IServiceCollection.AddMetricsMiddleware()\n");
            }
        }
    }
}