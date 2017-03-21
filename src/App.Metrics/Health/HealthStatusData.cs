// <copyright file="HealthStatusData.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace App.Metrics.Health
{
    public sealed class HealthStatusData
    {
        public Dictionary<string, string> Degraded { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> Healthy { get; set; } = new Dictionary<string, string>();

        public string Status { get; set; }

        public string Timestamp { get; set; }

        public Dictionary<string, string> Unhealthy { get; set; } = new Dictionary<string, string>();
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Global
}