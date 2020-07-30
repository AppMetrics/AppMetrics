// <copyright file="SocketSettings.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net;
using System.Net.Sockets;

namespace App.Metrics.Reporting.Socket.Client
{
    public class SocketSettings
    {
        public SocketSettings(ProtocolType protocolType, string address, int port)
        {
            Validate(protocolType, address, port);
            ProtocolType = protocolType;
            Address = address;
            Port = port;
        }

        public SocketSettings() { }

        /// <summary>
        ///     Gets or sets Protocol to send data.
        /// </summary>
        /// <value>
        ///     Possible variants are TCP and UDP.
        /// </value>
        public ProtocolType ProtocolType { get; set; }

        /// <summary>
        ///     Gets or sets Address to send data.
        /// </summary>
        /// <value>
        ///     Name of remote host.
        /// </value>
        public string Address { get; set; }

        /// <summary>
        ///     Gets or sets Port to send data.
        /// </summary>
        /// <value>
        ///     The remote port.
        /// </value>
        public int Port { get; set; }

        public string Endpoint
        {
            get
            {
                if (ProtocolType == ProtocolType.Tcp)
                {
                    return $"tcp://{Address}:{Port}";
                }

                if (ProtocolType == ProtocolType.Udp)
                {
                    return $"udp://{Address}:{Port}";
                }

                if (ProtocolType == ProtocolType.IP)
                {
                    return $"unix://{Address}";
                }

                return "Wrong Settings Instance";
            }
        }

        public static void Validate(
            ProtocolType protocolType,
            string address,
            int port)
        {
            if (protocolType != ProtocolType.Tcp
                && protocolType != ProtocolType.Udp
                && protocolType != ProtocolType.IP)
            {
                throw new ArgumentOutOfRangeException(nameof(protocolType), "Only TCP/IP/UDP protocols are available. IP only for Unix domain sockets");
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (protocolType == ProtocolType.IP)
            {
                if (port != 0)
                {
                    throw new ArgumentException(
                        $"Port must be 0 when Unix domain socket is used",
                        nameof(port));
                }

                return;
            }

            if (port <= IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(port),
                    port,
                    $"Port must be in ({IPEndPoint.MinPort}; {IPEndPoint.MaxPort}) range.");
            }

            string endpoint = $"{protocolType.ToString().ToLower()}://{address}:{port}";
            if (!Uri.TryCreate(endpoint, UriKind.Absolute, out var uri))
            {
                throw new InvalidOperationException($"{endpoint} must be a valid absolute URI.");
            }
        }
    }
}
