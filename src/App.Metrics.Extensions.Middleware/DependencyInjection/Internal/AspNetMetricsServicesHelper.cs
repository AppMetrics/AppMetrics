// <copyright file="AspNetMetricsServicesHelper.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Extensions.Middleware.DependencyInjection.Internal
{
    internal static class AspNetMetricsServicesHelper
    {
        /// <summary>
        ///     Throws InvalidOperationException when MetricsMarkerService is not present
        ///     in the list of services.
        /// </summary>
        /// <param name="services">The list of services.</param>
        public static void ThrowIfMetricsNotRegistered(IServiceProvider services)
        {
            if (services.GetService(typeof(AspNetMetricsMarkerService)) == null)
            {
                throw new InvalidOperationException(
                    "IServiceCollection.AddAspNetMetrics()\n");
            }
        }
    }
}