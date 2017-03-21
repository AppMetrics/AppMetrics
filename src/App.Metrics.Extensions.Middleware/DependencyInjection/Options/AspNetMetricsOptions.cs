// <copyright file="AspNetMetricsOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Extensions.Middleware.Internal;

namespace App.Metrics.Extensions.Middleware.DependencyInjection.Options
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable CollectionNeverUpdated.Global
    // ReSharper disable MemberCanBePrivate.Global
    public class AspNetMetricsOptions
    {
        public AspNetMetricsOptions()
        {
            HealthEndpointEnabled = true;
            MetricsEndpointEnabled = true;
            MetricsTextEndpointEnabled = true;
            PingEndpointEnabled = true;
            OAuth2TrackingEnabled = true;
            ApdexTrackingEnabled = true;
            ApdexTSeconds = Core.Internal.Constants.ReservoirSampling.DefaultApdexTSeconds;
        }

        public bool ApdexTrackingEnabled { get; set; }

        public double ApdexTSeconds { get; set; }

        public string HealthEndpoint { get; set; } = Constants.DefaultRoutePaths.HealthEndpoint.EnsureLeadingSlash();

        public bool HealthEndpointEnabled { get; set; }

        public IList<int> IgnoredHttpStatusCodes { get; set; } = new List<int>();

        public IList<string> IgnoredRoutesRegexPatterns { get; set; } = new List<string>();

        public string MetricsEndpoint { get; set; } = Constants.DefaultRoutePaths.MetricsEndpoint.EnsureLeadingSlash();

        public bool MetricsEndpointEnabled { get; set; }

        public string MetricsTextEndpoint { get; set; } = Constants.DefaultRoutePaths.MetricsTextEndpoint.EnsureLeadingSlash();

        public bool MetricsTextEndpointEnabled { get; set; }

        public bool OAuth2TrackingEnabled { get; set; }

        public string PingEndpoint { get; set; } = Constants.DefaultRoutePaths.PingEndpoint.EnsureLeadingSlash();

        public bool PingEndpointEnabled { get; set; }
        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
        // ReSharper restore CollectionNeverUpdated.Global
        // ReSharper restore MemberCanBePrivate.Global
    }
}