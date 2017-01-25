// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;

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
}