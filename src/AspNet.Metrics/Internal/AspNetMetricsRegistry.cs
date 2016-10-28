// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics;
using App.Metrics.Core;

namespace AspNet.Metrics.Internal
{
    internal static class AspNetMetricsRegistry
    {
        public static class Groups
        {
            public static class OAuth2
            { 
                //TODO: AH - alot of these metrics across groups are the same
                //TODO: AH - instead of having a separate metric per http status code, tag the metrics with the code instead
                public static string GroupName = "Application.OAuth2Client.WebRequests";

                public static class Meters
                {
                    public static Func<string, MeterOptions> EndpointWebRequests = routeTemplate => new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = routeTemplate,
                        MeasurementUnit = Unit.Requests,
                    };

                    public static MeterOptions WebRequests = new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = "Total Web Requests",
                        MeasurementUnit = Unit.Requests,
                    };

                    public static Func<string, MeterOptions> EndpointBadRequests = routeTemplate => new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = $"{routeTemplate} Bad Requests",
                        MeasurementUnit = Unit.Custom("400 Errors"),
                    };

                    public static MeterOptions TotalBadRequests = new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = "Total Bad Requests",
                        MeasurementUnit = Unit.Custom("400 Errors"),
                    };

                    public static Func<string, MeterOptions> EndpointUnAuthorizedRequests = routeTemplate => new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = $"{routeTemplate} UnAuthorized Requests",
                        MeasurementUnit = Unit.Custom("401 Errors"),
                    };

                    public static MeterOptions TotalUnAuthorizedRequests = new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = "Total UnAuthorized Requests",
                        MeasurementUnit = Unit.Custom("401 Errors"),
                    };

                    public static Func<string, MeterOptions> EndpointInternalErrorRequests = routeTemplate => new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = $"{routeTemplate} Internal Server Error Requests",
                        MeasurementUnit = Unit.Custom("500 Errors"),
                    };

                    public static MeterOptions TotalInternalErrorRequests = new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = "Total Internal Server Error Requests",
                        MeasurementUnit = Unit.Custom("500 Errors"),
                    };

                    public static Func<string, MeterOptions> EndpointTotalErrorRequests = routeTemplate => new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = $"{routeTemplate} Total Error Requests",
                        MeasurementUnit = Unit.Errors
                    };

                    public static MeterOptions TotalErrorRequests = new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = "Total Error Requests",
                        MeasurementUnit = Unit.Errors
                    };
                }
            }

            public static class WebRequests
            {
                public static string GroupName = "Application.WebRequests";

                public static class Counters
                {
                    public static CounterOptions ActiveRequests = new CounterOptions
                    {
                        GroupName = GroupName,
                        Name = "Active Requests",
                        MeasurementUnit = Unit.Custom("Active Requests")
                    };
                }

                public static class Histograms
                {
                    public static HistogramOptions PostAndPutRequestSize = new HistogramOptions
                    {
                        GroupName = GroupName,
                        Name = "Web Request Post & Put Size",
                        MeasurementUnit = Unit.Bytes
                    };
                }

                public static class Meters
                {
                    public static Func<string, MeterOptions> EndpointBadRequests = routeTemplate => new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = $"{routeTemplate} Bad Requests",
                        MeasurementUnit = Unit.Custom("400 Errors")
                    };

                    public static MeterOptions TotalBadRequests = new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = "Total Bad Requests",
                        MeasurementUnit = Unit.Custom("400 Errors")
                    };

                    public static Func<string, MeterOptions> EndpointUnAuthorizedRequests = routeTemplate => new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = $"{routeTemplate} UnAuthorized Requests",
                        MeasurementUnit = Unit.Custom("401 Errors"),
                    };

                    public static MeterOptions TotalUnAuthorizedRequests = new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = "Total UnAuthorized Requests",
                        MeasurementUnit = Unit.Custom("401 Errors"),
                    };

                    public static Func<string, MeterOptions> EndpointInternalServerErrorRequests = routeTemplate => new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = $"{routeTemplate} Internal Server Error Requests",
                        MeasurementUnit = Unit.Custom("500 Errors")
                    };

                    public static MeterOptions TotalInternalServerErrorRequests = new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = "Total Internal Server Error Requests",
                        MeasurementUnit = Unit.Custom("500 Errors")
                    };

                    public static Func<string, MeterOptions> EndpointTotalErrorRequests = routeTemplate => new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = $"{routeTemplate} Total Error Requests",
                        MeasurementUnit = Unit.Errors
                    };

                    public static MeterOptions TotalErrorRequests = new MeterOptions
                    {
                        GroupName = GroupName,
                        Name = "Total Error Requests",
                        MeasurementUnit = Unit.Errors
                    };
                }

                public static class Timers
                {
                    public static Func<string, TimerOptions> EndpointPerRequestTimer = routeTemplate => new TimerOptions
                    {
                        GroupName = GroupName,
                        Name = routeTemplate,
                        MeasurementUnit = Unit.Requests
                    };

                    public static TimerOptions WebRequestTimer = new TimerOptions
                    {
                        GroupName = GroupName,
                        Name = "Web Requests",
                        MeasurementUnit = Unit.Requests
                    };
                }
            }
        }
    }
}