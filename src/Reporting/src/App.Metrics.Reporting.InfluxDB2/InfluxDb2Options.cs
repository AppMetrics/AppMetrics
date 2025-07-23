// <copyright file="InfluxDBOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Reporting.InfluxDB2
{
    /// <summary>
    ///     Provides programmatic configuration for InfluxDB in the App Metrics framework.
    /// </summary>
    public class InfluxDb2Options
    {
        public InfluxDb2Options() {}


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
                if (string.IsNullOrWhiteSpace(Organization))
                {
                    return null;
                }
                if (string.IsNullOrWhiteSpace(Bucket))
                {
                    return null;
                }

                var endpoint = $"api/v2/write?org={Uri.EscapeDataString(Organization)}&bucket={Uri.EscapeDataString(Bucket)}";

                if (!string.IsNullOrWhiteSpace(Precision))
                {
                    endpoint += $"&precision={Uri.EscapeDataString(Precision)}";
                }

                return endpoint;
            }
        }

        /// <summary>
        ///     Gets or sets the base URI of the InfluxDB API.
        /// </summary>
        /// <value>
        ///     The base URI of the InfluxDB API where metrics are flushed.
        /// </value>
        public Uri BaseUri { get; set; }

        /// <summary>
        ///     Gets or sets the InfluxDB organization name used to report metrics.
        /// </summary>
        /// <value>
        ///     The InfluxDB organization name where metrics are flushed.
        /// </value>
        public string Organization { get; set; }
        /// <summary>
        ///     Gets or sets the InfluxDB bucket name used to report metrics.
        /// </summary>
        /// <value>
        ///     The InfluxDB bucket name where metrics are flushed.
        /// </value>
        public string Bucket { get; set; }

        /// <summary>
        ///     Gets or sets the precision for the unix timestamps within the body line-protocol.
        /// </summary>
        /// <value>
        ///     "ms" "s" "us" "ns"
        /// </value>
        public string Precision { get; set; }

        /// <summary>
        ///     Gets or sets the InfluxDB token.
        /// </summary>
        /// <value>
        ///     The InfluxDB Token.
        /// </value>
        public string Token { get; set; }

    }
}