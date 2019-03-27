// <copyright file="EnvironmentInfoProvider.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Globalization;

namespace App.Metrics.Infrastructure
{
    public sealed class EnvironmentInfoProvider
    {
        public EnvironmentInfo Build()
        {
            var localTimeString = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ffffK", CultureInfo.InvariantCulture);

            return new EnvironmentInfo(
                EnvironmentInfoProviderCache.Instance.RunningEnvironment,
                EnvironmentInfoProviderCache.Instance.FrameworkDescription,
                EnvironmentInfoProviderCache.Instance.EntryAssemblyName,
                EnvironmentInfoProviderCache.Instance.EntryAssemblyVersion,
                localTimeString,
                EnvironmentInfoProviderCache.Instance.MachineName,
                EnvironmentInfoProviderCache.Instance.OperatingSystemPlatform,
                EnvironmentInfoProviderCache.Instance.OperatingSystemVersion,
                EnvironmentInfoProviderCache.Instance.OperatingSystemArchitecture,
                EnvironmentInfoProviderCache.Instance.ProcessArchitecture,
                EnvironmentInfoProviderCache.Instance.ProcessorCount);
        }
    }
}