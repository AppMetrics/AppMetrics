// <copyright file="ConfigurationMetricsConfigurationExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Configuration;

namespace App.Metrics.Health.Extensions.Configuration
{
    /// <summary>
    /// Extends <see cref="ConfigurationMetricsConfigurationExtensions"/> with support for System.Configuration appSettings elements.
    /// </summary>
    public static class ConfigurationMetricsConfigurationExtensions
    {
        private const string DefaultSectionName = nameof(HealthOptions);

        public static IHealthBuilder ReadFrom(
            this IHealthConfigurationBuilder configurationBuilder,
            IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            configurationBuilder.ReadFrom(configuration.GetSection(DefaultSectionName));

            return configurationBuilder.Builder;
        }

        public static IHealthBuilder ReadFrom(
            this IHealthConfigurationBuilder configurationBuilder,
            IConfigurationSection configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            configurationBuilder.Extend(configuration.AsEnumerable());

            return configurationBuilder.Builder;
        }
    }
}
