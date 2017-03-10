// <copyright file="MetricsServicesHelper.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Internal;

namespace App.Metrics.DependencyInjection.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal static class MetricsServicesHelper
    {
        /// <summary>
        ///     Throws InvalidOperationException when MetricsMarkerService is not present
        ///     in the list of services.
        /// </summary>
        /// <param name="services">The list of services.</param>
        public static void ThrowIfMetricsNotRegistered(IServiceProvider services)
        {
            if (services.GetService(typeof(MetricsMarkerService)) == null)
            {
                throw new InvalidOperationException(
                    "IServiceCollection.AddMetrics()\n" +
                    "IApplicationBuilder.ConfigureServices(...)\n" +
                    "IApplicationBuilder.UseMetrics(...)\n");
            }
        }
    }
}