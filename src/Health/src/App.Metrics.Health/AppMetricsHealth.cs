// <copyright file="AppMetricsHealth.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Health.Builder;

namespace App.Metrics.Health
{
    /// <summary>
    /// Provides convenience methods for creating instances of <see cref="IHealth"/> and <see cref="IHealthBuilder"/> with pre-configured defaults.
    /// </summary>
    public static class AppMetricsHealth
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IHealthBuilder" /> class with pre-configured defaults.
        /// </summary>
        /// <remarks>
        ///     The following defaults are applied to the returned <see cref="IHealthBuilder" />:
        ///     use, JSON and Plain Text health check result formatting.
        /// </remarks>
        /// <returns>The initialized <see cref="IHealthBuilder" />.</returns>
        public static IHealthBuilder CreateDefaultBuilder()
        {
            var builder = new HealthBuilder()
                          .Configuration
                          .Configure(
                                options =>
                                {
                                    options.Enabled = true;
                                })
                           .OutputHealth.AsJson()
                           .OutputHealth.AsPlainText();

            return builder;
        }
    }
}
