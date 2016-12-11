// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using App.Metrics.Reporting.Interfaces;

namespace App.Metrics.Extensions.Reporting.InfluxDB
{
    public interface IInfluxDbReporterSettings : IReporterSettings
    {
        /// <summary>
        ///     Gets or sets the breaker rate, i.e. the error rate which temporarily stops writing data to InfluxDB.
        /// </summary>
        /// <example>3 / 00:00:30</example>
        /// <value>
        ///     The circuit breaker rate.
        /// </value>
        string BreakerRate { get; set; }

        /// <summary>
        ///     Gets or sets the number of InfluxDB notes that must confirm the write
        /// </summary>
        /// <value>
        ///     The InfluxDB node write consistency.
        /// </value>
        string Consistency { get; set; }

        /// <summary>
        ///     The InfluxDB database name where metrics will be persisted
        /// </summary>
        /// <value>
        ///     The InfluxDB database name.
        /// </value>
        string Database { get; set; }

        /// <summary>
        ///     Gets or sets the InfluxDB host.
        /// </summary>
        /// <value>
        ///     The InfluxDB host.
        /// </value>
        string BaseAddress { get; set; }

        /// <summary>
        ///     Gets or sets the InfluxDB database password.
        /// </summary>
        /// <value>
        ///     The InfluxDB database password.
        /// </value>
        string Password { get; set; }

        /// <summary>
        ///     Gets or sets the InfluxDB database's retention policy to target.
        /// </summary>
        /// <value>
        ///     The InfluxDB database's retention policy to target.
        /// </value>
        string RetentionPolicy { get; set; }

        /// <summary>
        ///     Gets or sets the InfluxDB database username.
        /// </summary>
        /// <value>
        ///     The InfluxDB database username.
        /// </value>
        string Username { get; set; }
    }
}