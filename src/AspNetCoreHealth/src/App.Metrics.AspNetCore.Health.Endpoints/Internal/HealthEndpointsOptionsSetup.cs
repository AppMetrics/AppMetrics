// <copyright file="HealthEndpointsOptionsSetup.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Health.Formatters;
using App.Metrics.Health.Formatters.Json;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Health.Endpoints.Internal
{
    /// <summary>
    ///     Sets up default health endpoint options for <see cref="HealthEndpointsOptions"/>.
    /// </summary>
    public class HealthEndpointsOptionsSetup : IConfigureOptions<HealthEndpointsOptions>
    {
        private readonly HealthFormatterCollection _healthFormatters;

        public HealthEndpointsOptionsSetup(IReadOnlyCollection<IHealthOutputFormatter> healthFormatters)
        {
            _healthFormatters = new HealthFormatterCollection(healthFormatters.ToList());
        }

        /// <inheritdoc />
        public void Configure(HealthEndpointsOptions options)
        {
            if (options.HealthEndpointOutputFormatter == null)
            {
                options.HealthEndpointOutputFormatter =
                    _healthFormatters.GetType<HealthStatusJsonOutputFormatter>() ?? _healthFormatters.LastOrDefault();
            }
        }
    }
}