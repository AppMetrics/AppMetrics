// <copyright file="SandboxMetricsRegistry.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Timer;

namespace GrafanaCloudHostedMetricsSandboxMvc
{
    public static class SandboxMetricsRegistry
    {
        public const string ContextName = "Sandbox";

        public static readonly TimerOptions DatabaseTimer = new TimerOptions
                                                            {
                                                                Context = ContextName,
                                                                Name = "Database Call"
                                                            };
    }
}