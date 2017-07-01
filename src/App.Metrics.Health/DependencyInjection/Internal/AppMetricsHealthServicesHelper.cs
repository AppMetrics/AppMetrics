// <copyright file="AppMetricsHealthServicesHelper.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;

namespace App.Metrics.Health.DependencyInjection.Internal
{
    [ExcludeFromCodeCoverage]
    internal static class AppMetricsHealthServicesHelper
    {
        /// <summary>
        ///     Throws InvalidOperationException when HealthCheckMarkerService is not present
        ///     in the list of services.
        /// </summary>
        /// <param name="services">The list of services.</param>
        public static void ThrowIfHealthChecksNotRegistered(IServiceProvider services)
        {
            if (services.GetService(typeof(HealthCheckMarkerService)) == null)
            {
                throw new InvalidOperationException(
                    "IServiceCollection.AddHealthChecks()\n" +
                    "IApplicationBuilder.ConfigureServices(...)\n" +
                    "IApplicationBuilder.UseHealthChecks(...)\n");
            }
        }
    }
}