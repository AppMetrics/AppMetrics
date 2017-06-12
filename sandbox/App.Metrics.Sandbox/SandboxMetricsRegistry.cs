// <copyright file="SandboxMetricsRegistry.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Timer;

namespace App.Metrics.Sandbox
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