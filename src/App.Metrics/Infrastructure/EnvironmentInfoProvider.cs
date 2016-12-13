// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using App.Metrics.Data;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Infrastructure
{
    public sealed class EnvironmentInfoProvider
    {
        public static string SafeGetString(Func<string> action)
        {
            try
            {
                return action();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<EnvironmentInfo> BuildAsync()
        {
            var process = Process.GetCurrentProcess();

            var processName = SafeGetString(() => process.ProcessName);
            var osVersion = RuntimeEnvironment.OperatingSystemVersion;
            var os = RuntimeEnvironment.OperatingSystem;
            var processorCount = Environment.ProcessorCount.ToString();
            var machineName = process.MachineName;
            var hostName = SafeGetString(Dns.GetHostName);
            var ipAddress = await GetIpAddressAsync();
            var localTimeString = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ffffK", CultureInfo.InvariantCulture);

            var entryAssembly = Assembly.GetEntryAssembly();

            var entryAssemblyName = SafeGetString(() => entryAssembly.GetName().Name);
            var entryAssemblyVersion = SafeGetString(() => entryAssembly.GetName().Version.ToString());


            return new EnvironmentInfo(entryAssemblyName, entryAssemblyVersion, hostName, ipAddress, localTimeString, machineName,
                os, osVersion, processName, processorCount);
        }

        private async Task<string> GetIpAddressAsync()
        {
            var hostName = SafeGetString(Dns.GetHostName);

            try
            {
                var ipAddress = await ResolveIpAddressAsync(hostName).ConfigureAwait(false);

                return ipAddress != null ? ipAddress.ToString() : string.Empty;
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode != SocketError.HostNotFound) throw;

                return string.Empty;
            }
        }

        private async Task<IPAddress> ResolveIpAddressAsync(string host)
        {
            var hostAddresses = await Dns.GetHostAddressesAsync(host).ConfigureAwait(false);

            var address = hostAddresses
                .Where(a => a.AddressFamily == AddressFamily.InterNetwork)
                .OrderBy(a => Guid.NewGuid())
                .FirstOrDefault();

            return address;
        }
    }
}