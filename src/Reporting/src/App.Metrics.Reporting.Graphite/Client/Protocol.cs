// <copyright file="Protocol.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Reporting.Graphite.Client
{
    public enum Protocol
    {
        /// <summary>
        /// Send data using TCP
        /// </summary>
        Tcp,

        /// <summary>
        /// Send data using UDP
        /// </summary>
        Udp,

        /// <summary>
        /// Send data using Pickled
        /// </summary>
        Pickled
    }
}