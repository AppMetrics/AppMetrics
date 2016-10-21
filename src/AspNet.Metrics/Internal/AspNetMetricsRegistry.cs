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
                public static string GroupName = "Application.OAuth2Client.WebRequests";

                public static class Meters
                {
                    public static Func<string, MeterOptions> EndpointWebRequests = routeTemplate => new MeterOptions
                    {
                        Name = routeTemplate,
                        MeasurementUnit = Unit.Requests,
                    };

                    public static MeterOptions WebRequests = new MeterOptions
                    {
                        Name = "Total Web Requests",
                        MeasurementUnit = Unit.Requests,
                    };

                    public static Func<string, MeterOptions> EndpointBadRequests = routeTemplate => new MeterOptions
                    {
                        Name = $"{routeTemplate} Bad Requests",
                        MeasurementUnit = Unit.Custom("400 Errors"),
                    };

                    public static MeterOptions TotalBadRequests = new MeterOptions
                    {
                        Name = "Total Bad Requests",
                        MeasurementUnit = Unit.Custom("400 Errors"),
                    };

                    public static Func<string, MeterOptions> EndpointUnAuthorizedRequests = routeTemplate => new MeterOptions
                    {
                        Name = $"{routeTemplate} UnAuthorized Requests",
                        MeasurementUnit = Unit.Custom("401 Errors"),
                    };

                    public static MeterOptions TotalUnAuthorizedRequests = new MeterOptions
                    {
                        Name = "Total UnAuthorized Requests",
                        MeasurementUnit = Unit.Custom("401 Errors"),
                    };

                    public static Func<string, MeterOptions> EndpointInternalErrorRequests = routeTemplate => new MeterOptions
                    {
                        Name = $"{routeTemplate} Internal Server Error Requests",
                        MeasurementUnit = Unit.Custom("500 Errors"),
                    };

                    public static MeterOptions TotalInternalErrorRequests = new MeterOptions
                    {
                        Name = "Total Internal Server Error Requests",
                        MeasurementUnit = Unit.Custom("500 Errors"),
                    };

                    public static Func<string, MeterOptions> EndpointTotalErrorRequests = routeTemplate => new MeterOptions
                    {
                        Name = $"{routeTemplate} Total Error Requests",
                        MeasurementUnit = Unit.Errors
                    };

                    public static MeterOptions TotalErrorRequests = new MeterOptions
                    {
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
                        Name = "Active Requests",
                        MeasurementUnit = Unit.Custom("Active Requests")
                    };
                }

                public static class Meters
                {
                    public static Func<string, MeterOptions> EndpointBadRequests = routeTemplate => new MeterOptions
                    {
                        Name = $"{routeTemplate} Bad Requests",
                        MeasurementUnit = Unit.Custom("400 Errors")
                    };

                    public static MeterOptions TotalBadRequests = new MeterOptions
                    {
                        Name = "Total Bad Requests",
                        MeasurementUnit = Unit.Custom("400 Errors")
                    };

                    public static Func<string, MeterOptions> EndpointUnAuthorizedRequests = routeTemplate => new MeterOptions
                    {
                        Name = $"{routeTemplate} UnAuthorized Requests",
                        MeasurementUnit = Unit.Custom("401 Errors"),
                    };

                    public static MeterOptions TotalUnAuthorizedRequests = new MeterOptions
                    {
                        Name = "Total UnAuthorized Requests",
                        MeasurementUnit = Unit.Custom("401 Errors"),
                    };

                    public static Func<string, MeterOptions> EndpointInternalServerErrorRequests = routeTemplate => new MeterOptions
                    {
                        Name = $"{routeTemplate} Internal Server Error Requests",
                        MeasurementUnit = Unit.Custom("500 Errors")
                    };

                    public static MeterOptions TotalInternalServerErrorRequests = new MeterOptions
                    {
                        Name = "Total Internal Server Error Requests",
                        MeasurementUnit = Unit.Custom("500 Errors")
                    };

                    public static Func<string, MeterOptions> EndpointTotalErrorRequests = routeTemplate => new MeterOptions
                    {
                        Name = $"{routeTemplate} Total Error Requests",
                        MeasurementUnit = Unit.Errors
                    };

                    public static MeterOptions TotalErrorRequests = new MeterOptions
                    {
                        Name = "Total Error Requests",
                        MeasurementUnit = Unit.Errors
                    };
                }
            }
        }
    }
}