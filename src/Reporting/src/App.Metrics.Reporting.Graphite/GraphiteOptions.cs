// <copyright file="GraphiteOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Reporting.Graphite.Client;

namespace App.Metrics.Reporting.Graphite
{
    public class GraphiteOptions
    {
        public GraphiteOptions(Uri baseUri)
        {
            BaseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
        }

        public GraphiteOptions()
        {
        }

        public Uri BaseUri { get; set; }

        /// <summary>
        ///     Gets the number of Graphite notes that must confirm the write
        /// </summary>
        /// <value>
        ///     The Graphite node write consistency.
        /// </value>
        /// <exception cref="System.ArgumentException">
        ///     Graphite URI scheme must be either net.tcp or net.udp or net.pickled - BaseAddress
        /// </exception>
        // ReSharper disable UnusedMember.Global
        public Protocol Protocol
            // ReSharper restore UnusedMember.Global
        {
            get
            {
                switch (BaseUri.Scheme.ToLowerInvariant())
                {
                    case "net.tcp":
                        return Protocol.Tcp;
                    case "net.udp":
                        return Protocol.Udp;
                    case "net.pickled":
                        return Protocol.Pickled;
                    default:
                        throw new ArgumentException("Graphite URI scheme must be either net.tcp or net.udp or net.pickled", nameof(BaseUri));
                }
            }
        }
    }
}
