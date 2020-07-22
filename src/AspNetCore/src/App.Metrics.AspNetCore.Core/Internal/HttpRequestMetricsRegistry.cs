// <copyright file="HttpRequestMetricsRegistry.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.AspNetCore.Internal
{
    [ExcludeFromCodeCoverage]
    internal static class HttpRequestMetricsRegistry
    {
#pragma warning disable SA1401
        public static string ContextName = "Application.HttpRequests";

        public static class ApdexScores
        {
            public static readonly string ApdexMetricName = "Apdex";

            public static readonly Func<double, ApdexOptions> Apdex = apdexTSeconds => new ApdexOptions
                                                                                       {
                                                                                           Context = ContextName,
                                                                                           Name = ApdexMetricName,
                                                                                           ApdexTSeconds = apdexTSeconds
                                                                                       };
        }

        public static class Counters
        {
            public static readonly CounterOptions ActiveRequestCount = new CounterOptions
                                                                       {
                                                                           Context = ContextName,
                                                                           Name = "Active",
                                                                           MeasurementUnit = Unit.Custom("Active Requests")
                                                                       };

            public static readonly CounterOptions ActiveWebSocketRequestCount = new CounterOptions
                                                                                {
                                                                                    Context = ContextName,
                                                                                    Name = "Active Web Sockets",
                                                                                    MeasurementUnit = Unit.Custom("Active Requests")
                                                                                };

            public static readonly CounterOptions TotalErrorRequestCount = new CounterOptions
                                                                           {
                                                                               Context = ContextName,
                                                                               Name = "Errors",
                                                                               ResetOnReporting = true,
                                                                               MeasurementUnit = Unit.Errors
                                                                           };

            public static readonly CounterOptions UnhandledExceptionCount = new CounterOptions
                                                                            {
                                                                                Context = ContextName,
                                                                                Name = "Exceptions",
                                                                                MeasurementUnit = Unit.Errors,
                                                                                ReportItemPercentages = false,
                                                                                ReportSetItems = false,
                                                                                ResetOnReporting = true
                                                                            };
        }

        public static class Gauges
        {
            public static readonly GaugeOptions EndpointOneMinuteErrorPercentageRate = new GaugeOptions
                                                                                       {
                                                                                           Context = ContextName,
                                                                                           Name = "One Minute Error Percentage Rate Per Endpoint",
                                                                                           MeasurementUnit = Unit.Requests
                                                                                       };

            public static readonly GaugeOptions OneMinErrorPercentageRate = new GaugeOptions
                                                                            {
                                                                                Context = ContextName,
                                                                                Name = "One Minute Error Percentage Rate",
                                                                                MeasurementUnit = Unit.Requests
                                                                            };
        }

        public static class Histograms
        {
            public static readonly HistogramOptions PostRequestSizeHistogram = new HistogramOptions
                                                                               {
                                                                                   Context = ContextName,
                                                                                   Name = "POST Size",
                                                                                   MeasurementUnit = Unit.Bytes
                                                                               };

            public static readonly HistogramOptions PutRequestSizeHistogram = new HistogramOptions
                                                                              {
                                                                                  Context = ContextName,
                                                                                  Name = "PUT Size",
                                                                                  MeasurementUnit = Unit.Bytes
                                                                              };
        }

        public static class BucketHistograms
        {
            public static readonly Func<double[], BucketHistogramOptions> PostRequestSizeHistogram = buckets => new BucketHistogramOptions
            {
                                                                                   Context = ContextName,
                                                                                   Name = "POST Size",
                                                                                   MeasurementUnit = Unit.Bytes,
                                                                                   Buckets = buckets
                                                                               };

            public static readonly Func<double[], BucketHistogramOptions> PutRequestSizeHistogram = buckets => new BucketHistogramOptions
            {
                                                                                  Context = ContextName,
                                                                                  Name = "PUT Size",
                                                                                  MeasurementUnit = Unit.Bytes,
                                                                                  Buckets = buckets
                                                                              };
        }

        public static class Meters
        {
            public static readonly MeterOptions EndpointErrorRequestPerStatusCodeRate = new MeterOptions
                                                                                        {
                                                                                            Context = ContextName,
                                                                                            Name = "Error Rate Per Endpoint And Status Code",
                                                                                            MeasurementUnit = Unit.Requests
                                                                                        };

            public static readonly MeterOptions EndpointErrorRequestRate = new MeterOptions
                                                                           {
                                                                               Context = ContextName,
                                                                               Name = "Error Rate Per Endpoint",
                                                                               MeasurementUnit = Unit.Requests
                                                                           };

            public static readonly MeterOptions ErrorRequestRate = new MeterOptions
                                                                   {
                                                                       Context = ContextName,
                                                                       Name = "Error Rate",
                                                                       MeasurementUnit = Unit.Requests
                                                                   };
        }

        public static class Timers
        {
            public static readonly TimerOptions EndpointRequestTransactionDuration = new TimerOptions
                                                                                     {
                                                                                         Context = ContextName,
                                                                                         Name = "Transactions Per Endpoint",
                                                                                         MeasurementUnit = Unit.Requests
                                                                                     };

            public static readonly TimerOptions RequestTransactionDuration = new TimerOptions
                                                                             {
                                                                                 Context = ContextName,
                                                                                 Name = "Transactions",
                                                                                 MeasurementUnit = Unit.Requests
                                                                             };
        }

        public static class BucketTimers
        {
            public static readonly Func<double[], BucketTimerOptions> EndpointRequestTransactionDuration = buckets => new BucketTimerOptions
                                                                                     {
                                                                                         Context = ContextName,
                                                                                         Name = "Transactions Per Endpoint",
                                                                                         MeasurementUnit = Unit.Requests,
                                                                                         Buckets = buckets
                                                                                     };

            public static readonly Func<double[], BucketTimerOptions> RequestTransactionDuration = buckets => new BucketTimerOptions
                                                                             {
                                                                                 Context = ContextName,
                                                                                 Name = "Transactions",
                                                                                 MeasurementUnit = Unit.Requests,
                                                                                 Buckets = buckets
                                                                             };
        }
    }

#pragma warning restore SA1401
}