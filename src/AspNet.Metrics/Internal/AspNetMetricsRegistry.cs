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
                //TODO: AH - alot of these metrics across contexts are the same
                //TODO: AH - instead of having a separate metric per http status code, tag the metrics with the code instead
                public static string ContextName = "Application.OAuth2Client.WebRequests";

                public static class Meters
                {
                    public static Func<string, MeterOptions> EndpointBadRequests = routeTemplate => new MeterOptions
                    {
                        Context = ContextName,
                        Name = $"{routeTemplate} Bad Requests",
                        MeasurementUnit = Unit.Custom("400 Errors"),
                    };

                    public static Func<string, MeterOptions> EndpointInternalErrorRequests = routeTemplate => new MeterOptions
                    {
                        Context = ContextName,
                        Name = $"{routeTemplate} Internal Server Error Requests",
                        MeasurementUnit = Unit.Custom("500 Errors"),
                    };

                    public static Func<string, MeterOptions> EndpointTotalErrorRequests = routeTemplate => new MeterOptions
                    {
                        Context = ContextName,
                        Name = $"{routeTemplate} Total Error Requests",
                        MeasurementUnit = Unit.Errors
                    };

                    public static Func<string, MeterOptions> EndpointUnAuthorizedRequests = routeTemplate => new MeterOptions
                    {
                        Context = ContextName,
                        Name = $"{routeTemplate} UnAuthorized Requests",
                        MeasurementUnit = Unit.Custom("401 Errors"),
                    };

                    public static Func<string, MeterOptions> EndpointWebRequests = routeTemplate => new MeterOptions
                    {
                        Context = ContextName,
                        Name = routeTemplate,
                        MeasurementUnit = Unit.Requests,
                    };

                    public static MeterOptions TotalBadRequests = new MeterOptions
                    {
                        Context = ContextName,
                        Name = "Total Bad Requests",
                        MeasurementUnit = Unit.Custom("400 Errors"),
                    };

                    public static MeterOptions TotalErrorRequests = new MeterOptions
                    {
                        Context = ContextName,
                        Name = "Total Error Requests",
                        MeasurementUnit = Unit.Errors
                    };

                    public static MeterOptions TotalInternalErrorRequests = new MeterOptions
                    {
                        Context = ContextName,
                        Name = "Total Internal Server Error Requests",
                        MeasurementUnit = Unit.Custom("500 Errors"),
                    };

                    public static MeterOptions TotalUnAuthorizedRequests = new MeterOptions
                    {
                        Context = ContextName,
                        Name = "Total UnAuthorized Requests",
                        MeasurementUnit = Unit.Custom("401 Errors"),
                    };

                    public static MeterOptions WebRequests = new MeterOptions
                    {
                        Context = ContextName,
                        Name = "Total Web Requests",
                        MeasurementUnit = Unit.Requests,
                    };
                }
            }

            public static class WebRequests
            {
                public static string Context = "Application.WebRequests";

                public static class Counters
                {
                    public static CounterOptions ActiveRequests = new CounterOptions
                    {
                        Context = Context,
                        Name = "Active Requests",
                        MeasurementUnit = Unit.Custom("Active Requests")
                    };
                }

                public static class Histograms
                {
                    public static HistogramOptions PostAndPutRequestSize = new HistogramOptions
                    {
                        Context = Context,
                        Name = "Web Request Post & Put Size",
                        MeasurementUnit = Unit.Bytes
                    };
                }

                public static class Meters
                {
                    public static Func<string, MeterOptions> EndpointBadRequests = routeTemplate => new MeterOptions
                    {
                        Context = Context,
                        Name = $"{routeTemplate} Bad Requests",
                        MeasurementUnit = Unit.Custom("400 Errors")
                    };

                    public static Func<string, MeterOptions> EndpointInternalServerErrorRequests = routeTemplate => new MeterOptions
                    {
                        Context = Context,
                        Name = $"{routeTemplate} Internal Server Error Requests",
                        MeasurementUnit = Unit.Custom("500 Errors")
                    };

                    public static Func<string, MeterOptions> EndpointTotalErrorRequests = routeTemplate => new MeterOptions
                    {
                        Context = Context,
                        Name = $"{routeTemplate} Total Error Requests",
                        MeasurementUnit = Unit.Errors
                    };

                    public static Func<string, MeterOptions> EndpointUnAuthorizedRequests = routeTemplate => new MeterOptions
                    {
                        Context = Context,
                        Name = $"{routeTemplate} UnAuthorized Requests",
                        MeasurementUnit = Unit.Custom("401 Errors"),
                    };

                    public static MeterOptions TotalBadRequests = new MeterOptions
                    {
                        Context = Context,
                        Name = "Total Bad Requests",
                        MeasurementUnit = Unit.Custom("400 Errors")
                    };

                    public static MeterOptions TotalErrorRequests = new MeterOptions
                    {
                        Context = Context,
                        Name = "Total Error Requests",
                        MeasurementUnit = Unit.Errors
                    };

                    public static MeterOptions TotalInternalServerErrorRequests = new MeterOptions
                    {
                        Context = Context,
                        Name = "Total Internal Server Error Requests",
                        MeasurementUnit = Unit.Custom("500 Errors")
                    };

                    public static MeterOptions TotalUnAuthorizedRequests = new MeterOptions
                    {
                        Context = Context,
                        Name = "Total UnAuthorized Requests",
                        MeasurementUnit = Unit.Custom("401 Errors"),
                    };
                }

                public static class Timers
                {
                    public static Func<string, TimerOptions> EndpointPerRequestTimer = routeTemplate => new TimerOptions
                    {
                        Context = Context,
                        Name = routeTemplate,
                        MeasurementUnit = Unit.Requests
                    };

                    public static TimerOptions WebRequestTimer = new TimerOptions
                    {
                        Context = Context,
                        Name = "Web Requests",
                        MeasurementUnit = Unit.Requests
                    };
                }
            }
        }
    }
}