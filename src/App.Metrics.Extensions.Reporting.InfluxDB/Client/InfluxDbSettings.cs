// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace App.Metrics.Extensions.Reporting.InfluxDB.Client
{
    // ReSharper disable InconsistentNaming
    public class InfluxDBSettings
        // ReSharper restore InconsistentNaming
    {
        public InfluxDBSettings(string database, Uri baseAddress)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            if (database.IsMissing())
            {
                throw new ArgumentException("A database must be specified", nameof(database));
            }

            if (baseAddress == null)
            {
                throw new ArgumentNullException(nameof(baseAddress));
            }

            Database = database;
            BaseAddress = baseAddress;
        }

        internal InfluxDBSettings() { }

        /// <summary>
        ///     Gets or sets the InfluxDB host.
        /// </summary>
        /// <value>
        ///     The InfluxDB host.
        /// </value>
        public Uri BaseAddress { get; set; }

        /// <summary>
        ///     Gets or sets the number of InfluxDB notes that must confirm the write
        /// </summary>
        /// <value>
        ///     The InfluxDB node write consistency.
        /// </value>
        public string Consistenency { get; set; }

        /// <summary>
        ///     Gets or sets the InfluxDB database name where metrics will be persisted.
        /// </summary>
        /// <value>
        ///     The InfluxDB database name.
        /// </value>
        public string Database { get; set; }

        /// <summary>
        ///     Gets formatted endpoint for writes to InfluxDB
        /// </summary>
        /// <value>
        ///     The InfluxDB endpoint for writes.
        /// </value>
        public string Endpoint
        {
            get
            {
                var endpoint = $"write?db={Uri.EscapeDataString(Database)}";

                if (RetensionPolicy.IsPresent())
                {
                    endpoint += $"&rp={Uri.EscapeDataString(RetensionPolicy)}";
                }

                if (Consistenency.IsPresent())
                {
                    endpoint += $"&consistency={Uri.EscapeDataString(Consistenency)}";
                }

                return endpoint;
            }
        }

        /// <summary>
        ///     Gets or sets the InfluxDB database password.
        /// </summary>
        /// <value>
        ///     The InfluxDB database password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        ///     Gets or sets the InfluxDB database's retention policy to target.
        /// </summary>
        /// <value>
        ///     The InfluxDB database's retention policy to target.
        /// </value>
        public string RetensionPolicy { get; set; }

        /// <summary>
        ///     Gets or sets the InfluxDB database username.
        /// </summary>
        /// <value>
        ///     The InfluxDB database username.
        /// </value>
        public string UserName { get; set; }
    }
}