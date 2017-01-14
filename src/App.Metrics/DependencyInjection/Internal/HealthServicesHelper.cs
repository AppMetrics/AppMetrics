// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace App.Metrics.DependencyInjection.Internal
{
    internal static class HealthServicesHelper
    {
        /// <summary>
        ///     Throws InvalidOperationException when MetricsMarkerService is not present
        ///     in the list of services.
        /// </summary>
        /// <param name="services">The list of services.</param>
        public static void ThrowIfMetricsNotRegistered(IServiceProvider services)
        {
            if (services.GetService(typeof(HealthCheckMarkerService)) == null)
            {
                throw new InvalidOperationException(
                    "IServiceCollection.AddMetrics().AddHealthChecks()\n" +
                    "IApplicationBuilder.ConfigureServices(...)\n" +
                    "IApplicationBuilder.UseMetrics(...)\n");
            }
        }
    }
}