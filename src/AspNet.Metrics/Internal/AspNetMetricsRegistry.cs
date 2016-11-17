// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics;
using App.Metrics.Core.Options;

namespace AspNet.Metrics.Internal
{
    internal static class AspNetMetricsRegistry
    {
        public static class Contexts
        {
            public static class OAuth2
            {
                public static string ContextName = "Application.OAuth2Client.HttpRequests";

                public static class Meters
                {
                    public static Func<string, MeterOptions> EndpointHttpRequests = routeTemplate => new MeterOptions
                    {
                        Context = ContextName,
                        Name = $"{routeTemplate} Http Requests",
                        MeasurementUnit = Unit.Requests
                    };

                    public static MeterOptions HttpRequests = new MeterOptions
                    {
                        Context = ContextName,
                        Name = "Http Requests",
                        MeasurementUnit = Unit.Requests
                    };

                    public static MetricItem HttpRequestSubItemId = new MetricItem()
                        .With("client_id", "")
                        .With("http_status_code", "");
                }
            }

            public static class HttpRequests
            {
                public static string ContextName = "Application.HttpRequests";

                public static class Counters
                {
                    public static CounterOptions ActiveRequests = new CounterOptions
                    {
                        Context = ContextName,
                        Name = "Active Requests",
                        MeasurementUnit = Unit.Custom("Active Requests")
                    };
                }

                public static class Histograms
                {
                    public static HistogramOptions PostAndPutRequestSize = new HistogramOptions
                    {
                        Context = ContextName,
                        Name = "Web Request Post & Put Size",
                        MeasurementUnit = Unit.Bytes
                    };
                }

                public static class Meters
                {
                    public static Func<string, MeterOptions> EndpointHttpErrorRequests = routeTemplate => new MeterOptions
                    {
                        Context = ContextName,
                        Name = $"{routeTemplate} Http Error Requests",
                        MeasurementUnit = Unit.Requests
                    };

                    public static MeterOptions HttpErrorRequests = new MeterOptions
                    {
                        Context = ContextName,
                        Name = "Http Error Requests",
                        MeasurementUnit = Unit.Requests
                    };
                }

                public static class Timers
                {
                    public static Func<string, TimerOptions> EndpointPerRequestTimer = routeTemplate => new TimerOptions
                    {
                        Context = ContextName,
                        Name = routeTemplate,
                        MeasurementUnit = Unit.Requests
                    };

                    public static TimerOptions WebRequestTimer = new TimerOptions
                    {
                        Context = ContextName,
                        Name = "Http Requests",
                        MeasurementUnit = Unit.Requests
                    };
                }
            }
        }
    }
}