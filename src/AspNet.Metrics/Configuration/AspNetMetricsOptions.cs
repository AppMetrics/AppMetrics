// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using AspNet.Metrics.Internal;

namespace AspNet.Metrics.Configuration
{
    public class AspNetMetricsOptions
    {
        public AspNetMetricsOptions()
        {
            HealthEndpointEnabled = true;
            MetricsEndpointEnabled = true;
            MetricsTextEndpointEnabled = true;
            PingEndpointEnabled = true;
            OAuth2TrackingEnabled = true;
        }

        public string HealthEndpoint { get; set; } = Constants.DefaultRoutePaths.HealthEndpoint.EnsureLeadingSlash();

        public bool HealthEndpointEnabled { get; set; }

        public IList<string> IgnoredRoutesRegexPatterns { get; set; } = new List<string>();

        public string MetricsEndpoint { get; set; } = Constants.DefaultRoutePaths.MetricsEndpoint.EnsureLeadingSlash();

        public bool MetricsEndpointEnabled { get; set; }

        public string MetricsTextEndpoint { get; set; } = Constants.DefaultRoutePaths.MetricsTextEndpoint.EnsureLeadingSlash();

        public bool MetricsTextEndpointEnabled { get; set; }

        public bool OAuth2TrackingEnabled { get; set; }

        public string PingEndpoint { get; set; } = Constants.DefaultRoutePaths.PingEndpoint.EnsureLeadingSlash();

        public bool PingEndpointEnabled { get; set; }
    }
}