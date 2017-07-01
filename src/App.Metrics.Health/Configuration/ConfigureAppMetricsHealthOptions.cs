// <copyright file="ConfigureAppMetricsHealthOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.Extensions.Options;

namespace App.Metrics.Health.Configuration
{
    public class ConfigureAppMetricsHealthOptions : IConfigureOptions<AppMetricsHealthOptions>
    {
        /// <inheritdoc />
        public void Configure(AppMetricsHealthOptions options)
        {
            // Nothing yet
        }
    }
}