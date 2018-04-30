// <copyright file="AppMetricsOptionExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Infrastructure;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsOptionExtensions
    {
        private static readonly EnvironmentInfo EnvInfo = new EnvironmentInfoProvider().Build();

        public static MetricsOptions WithGlobalTags(
            this MetricsOptions options,
            Action<Dictionary<string, string>, EnvironmentInfo> setupAction)
        {
            setupAction(options.GlobalTags, EnvInfo);

            return options;
        }

        public static MetricsOptions AddAppTag(this MetricsOptions options, string appName = null)
        {
            if (!options.GlobalTags.ContainsKey("app"))
            {
                options.GlobalTags.Add("app", appName ?? EnvInfo.EntryAssemblyName);
            }
            else
            {
                options.GlobalTags["app"] = appName ?? EnvInfo.EntryAssemblyName;
            }

            return options;
        }

        public static MetricsOptions AddServerTag(this MetricsOptions options, string serverName = null)
        {
            if (!options.GlobalTags.ContainsKey("server"))
            {
                options.GlobalTags.Add("server", serverName ?? EnvInfo.MachineName);
            }
            else
            {
                options.GlobalTags["server"] = serverName ?? EnvInfo.MachineName;
            }

            return options;
        }

        public static MetricsOptions AddEnvTag(this MetricsOptions options, string envName = null)
        {
            if (!options.GlobalTags.ContainsKey("env"))
            {
                options.GlobalTags.Add("env", envName ?? EnvInfo.RunningEnvironment);
            }
            else
            {
                options.GlobalTags["env"] = envName ?? EnvInfo.RunningEnvironment;
            }

            return options;
        }
    }
}