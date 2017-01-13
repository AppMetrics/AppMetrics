// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace App.Metrics
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