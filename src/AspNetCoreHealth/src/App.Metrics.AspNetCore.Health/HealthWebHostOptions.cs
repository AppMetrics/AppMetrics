// <copyright file="HealthWebHostOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.AspNetCore.Health.Endpoints;
using App.Metrics.Health;

namespace App.Metrics.AspNetCore.Health
{
    /// <summary>
    ///     Provides programmatic configuration for health and health endpoints in the App Metrics framework.
    /// </summary>
    public class HealthWebHostOptions
    {
        public HealthWebHostOptions()
        {
            HealthOptions = options => { };
            EndpointOptions = options => { };
        }

        /// <summary>
        ///     Gets or sets <see cref="Action{HealthOptions}" /> to configure the provided <see cref="HealthOptions" />.
        /// </summary>
        public Action<HealthOptions> HealthOptions { get; set; }

        /// <summary>
        ///     Gets or sets <see cref="Action{HealthEndpointsOptions}" /> to configure the provided <see cref="EndpointOptions" />.
        /// </summary>
        public Action<HealthEndpointsOptions> EndpointOptions { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="EventHandler" /> registered with an exception is thrown.
        /// </summary>
        public EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskExceptionHandler { get; set; }
    }
}