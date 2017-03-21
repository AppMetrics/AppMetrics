// <copyright file="EnvironmentInfoProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

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
            var localTimeString = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ffffK", CultureInfo.InvariantCulture);

            return new EnvironmentInfo(
                EnvironmentInfoProviderCache.Instance.EntryAssemblyName,
                EnvironmentInfoProviderCache.Instance.EntryAssemblyVersion,
                EnvironmentInfoProviderCache.Instance.HostName,
                localTimeString,
                EnvironmentInfoProviderCache.Instance.MachineName,
                EnvironmentInfoProviderCache.Instance.Os,
                EnvironmentInfoProviderCache.Instance.OperatingSystemVersion,
                EnvironmentInfoProviderCache.Instance.ProcessName,
                EnvironmentInfoProviderCache.Instance.ProcessorCount);
        }
    }
}