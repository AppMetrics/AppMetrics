// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection;
using Microsoft.DotNet.PlatformAbstractions;

namespace App.Metrics.Infrastructure
{
    public sealed class EnvironmentInfoProvider
    {
        public EnvironmentInfo Build()
        {
            var process = Process.GetCurrentProcess();

            var processName = StringExtensions.GetSafeString(() => process.ProcessName);
            var operatingSystemVersion = RuntimeEnvironment.OperatingSystemVersion;
            var os = RuntimeEnvironment.OperatingSystem;
            var processorCount = Environment.ProcessorCount.ToString();
            var machineName = process.MachineName;
            var hostName = StringExtensions.GetSafeString(Dns.GetHostName);
            var localTimeString = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ffffK", CultureInfo.InvariantCulture);

            var entryAssembly = Assembly.GetEntryAssembly();

            var entryAssemblyName = StringExtensions.GetSafeString(() => entryAssembly.GetName().Name);
            var entryAssemblyVersion = StringExtensions.GetSafeString(() => entryAssembly.GetName().Version.ToString());

            return new EnvironmentInfo(
                entryAssemblyName,
                entryAssemblyVersion,
                hostName,
                localTimeString,
                machineName,
                os,
                operatingSystemVersion,
                processName,
                processorCount);
        }
    }
}