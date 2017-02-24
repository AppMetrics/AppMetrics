// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Options;

namespace App.Metrics.Extensions.Middleware.Internal
{
    internal static class HttpRequestMetricsRegistry
    {
#pragma warning disable SA1401
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

        public static class Counters
        {
            public static CounterOptions ActiveRequests = new CounterOptions
                                                          {
                                                              Context = ContextName,
                                                              Name = "Active Requests",
                                                              MeasurementUnit = Unit.Custom("Active Requests")
                                                          };
        }

        public static class Gauges
        {
            public static GaugeOptions EndpointPercentageErrorRequests = new GaugeOptions
                                                                         {
                                                                             Context = ContextName,
                                                                             Name = "Endpoint Percentage Error Requests",
                                                                             MeasurementUnit = Unit.Errors
                                                                         };

            public static GaugeOptions PercentageErrorRequests = new GaugeOptions
                                                                 {
                                                                     Context = ContextName,
                                                                     Name = "Percentage Error Requests",
                                                                     MeasurementUnit = Unit.Custom("Error Requests")
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
            public static MeterOptions EndpointHttpErrorRequests = new MeterOptions
                                                                   {
                                                                       Context = ContextName,
                                                                       Name = "Http Error Request Transactions",
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
            public static TimerOptions HttpRequestTransactions = new TimerOptions
                                                                 {
                                                                     Context = ContextName,
                                                                     Name = "Http Request Transactions",
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

#pragma warning restore SA1401
}