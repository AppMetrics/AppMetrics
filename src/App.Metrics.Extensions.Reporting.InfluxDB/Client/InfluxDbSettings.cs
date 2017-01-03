using System;

namespace App.Metrics.Extensions.Reporting.InfluxDB.Client
{
    public class InfluxDBSettings
    {
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
        ///     The InfluxDB database name where metrics will be persisted
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