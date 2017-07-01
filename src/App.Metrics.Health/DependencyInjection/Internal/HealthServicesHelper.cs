// <copyright file="HealthServicesHelper.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;

namespace App.Metrics.Health.DependencyInjection.Internal
{
    [ExcludeFromCodeCoverage]
    internal static class HealthServicesHelper
    {
        /// <summary>
        ///     Throws InvalidOperationException when MetricsMarkerService is not present
        ///     in the list of services.
        /// </summary>
        /// <param name="services">The list of services.</param>
        public static void ThrowIfHealthChecksNotRegistered(IServiceProvider services)
        {
            if (services.GetService(typeof(HealthCheckMarkerService)) == null)
            {
                throw new InvalidOperationException("IServiceCollection.AddHealthChecks() needs to be configured on Startup");
            }
        }

        public static void ThrowIfHealthAddChecksHasAlreadyBeenCalled(IServiceProvider services)
        {
            if (services.GetService(typeof(HealthCheckFluentMarkerService)) != null)
            {
                throw new InvalidOperationException("IServiceCollection.AddHealthChecks().AddChecks() can only be called once.");
            }
        }
    }
}