// <copyright file="HealthOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Health
{
    /// <summary>
    ///     Top level container for all configuration settings of Health
    /// </summary>
    public class HealthOptions
    {
        public HealthOptions()
        {
            Enabled = true;
            ReportingEnabled = true;
        }

        /// <summary>
        ///     Gets or sets the application name used when reporting health status for example.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [health checks enabled]. This will also avoid registering all health
        ///     middleware if using App.Metrics.Health.Middleware.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [health enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [reporting enabled].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [reporting enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool ReportingEnabled { get; set; } = true;
    }
}