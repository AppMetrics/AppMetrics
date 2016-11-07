// // Copyright (c) Allan Hardy & Asif Mushtaq. All rights reserved.
// // Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace App.Metrics
{
    public sealed class HealthStatusData
    {
        public Dictionary<string, string> Healthy { get; set; }

        public bool IsHealthy { get; set; }

        public string Timestamp { get; set; }

        public Dictionary<string, string> Unhealthy { get; set; }
    }
}