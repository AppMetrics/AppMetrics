// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Core.Options;

namespace App.Metrics.Extensions.Owin.Internal
{
    internal static class OwinMetricsRegistry
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
                }
            }

            public static class HttpRequests
            {
                public static string ContextName = "Application.HttpRequests";               

                public static class ApdexScores
                {
                    public static readonly string ApdexMetricName = "Apdex";

                    public static Func<double, ApdexOptions> Apdex = apdexTSeconds => new ApdexOptions
                    {
                        Context = ContextName,
                        Name = ApdexMetricName,
                        ApdexTSeconds = apdexTSeconds
                    };
                }

                public static class Gauges
                {
                    public static GaugeOptions PercentageErrorRequests = new GaugeOptions
                    {
                        Context = ContextName,
                        Name = "Percentage Error Requests",
                        MeasurementUnit = Unit.Custom("Error Requests")
                    };                    
                }

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
                        Name = "Http Request Post & Put Size",
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