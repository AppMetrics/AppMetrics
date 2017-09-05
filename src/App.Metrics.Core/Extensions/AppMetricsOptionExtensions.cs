// <copyright file="AppMetricsOptionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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
        private static readonly EnvironmentInfo EnvironmentBuilder = new EnvironmentInfoProvider().Build();

        public static MetricsOptions WithGlobalTags(
            this MetricsOptions options,
            Action<Dictionary<string, string>, EnvironmentInfo> setupAction)
        {
            setupAction(options.GlobalTags, EnvironmentBuilder);

            return options;
        }

        public static MetricsOptions AddAppTag(this MetricsOptions options, string appName = null)
        {
            if (!options.GlobalTags.ContainsKey("app"))
            {
                options.GlobalTags.Add("app", appName ?? EnvironmentBuilder.EntryAssemblyName);
            }
            else
            {
                options.GlobalTags["app"] = appName ?? EnvironmentBuilder.EntryAssemblyName;
            }

            return options;
        }

        public static MetricsOptions AddServerTag(this MetricsOptions options, string serverName = null)
        {
            if (!options.GlobalTags.ContainsKey("server"))
            {
                options.GlobalTags.Add("server", serverName ?? EnvironmentBuilder.MachineName);
            }
            else
            {
                options.GlobalTags["server"] = serverName ?? EnvironmentBuilder.MachineName;
            }

            return options;
        }

        public static MetricsOptions AddEnvTag(this MetricsOptions options, string envName = null)
        {
            if (!options.GlobalTags.ContainsKey("env"))
            {
#if DEBUG
                options.GlobalTags.Add("env", envName ?? "debug");
#else
                options.GlobalTags.Add("env", envName ?? "release");
#endif
            }
            else
            {
#if DEBUG
                options.GlobalTags["env"] = envName ?? "debug";
#else
                options.GlobalTags["env"] = envName ?? "release";
#endif
            }

            return options;
        }
    }
}