// <copyright file="HttpRequestMetricsRegistry.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

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

            public static readonly Func<double, ApdexOptions> Apdex = apdexTSeconds => new ApdexOptions
                                                                                       {
                                                                                           Context = ContextName,
                                                                                           Name = ApdexMetricName,
                                                                                           ApdexTSeconds = apdexTSeconds
                                                                                       };
        }

        public static class Counters
        {
            public static readonly CounterOptions ActiveRequests = new CounterOptions
                                                                   {
                                                                       Context = ContextName,
                                                                       Name = "Active Requests",
                                                                       MeasurementUnit = Unit.Custom("Active Requests")
                                                                   };

            public static readonly CounterOptions ErrorRequests = new CounterOptions
                                                                  {
                                                                      Context = ContextName,
                                                                      Name = "Error Requests",
                                                                      ResetOnReporting = true,
                                                                      MeasurementUnit = Unit.Custom("Error Requests")
                                                                  };
        }

        public static class Gauges
        {
            public static readonly GaugeOptions OverallPercentageErrorRequests = new GaugeOptions
                                                                                 {
                                                                                     Context = ContextName,
                                                                                     Name = "Overall Percentage Error Requests",
                                                                                     MeasurementUnit = Unit.Custom("Error Requests")
                                                                                 };

            public static readonly GaugeOptions PercentageErrorRequests = new GaugeOptions
                                                                          {
                                                                              Context = ContextName,
                                                                              Name = "Percentage Error Requests",
                                                                              MeasurementUnit = Unit.Custom("Error Requests")
                                                                          };
        }

        public static class Histograms
        {
            public static readonly HistogramOptions PostAndPutRequestSize = new HistogramOptions
                                                                            {
                                                                                Context = ContextName,
                                                                                Name = "Http Request Post & Put Size",
                                                                                MeasurementUnit = Unit.Bytes
                                                                            };
        }

        public static class Meters
        {
            public static readonly MeterOptions HttpErrorRequests = new MeterOptions
                                                                    {
                                                                        Context = ContextName,
                                                                        Name = "Http Error Requests",
                                                                        MeasurementUnit = Unit.Requests
                                                                    };

            public static readonly MeterOptions TotalHttpErrorRequests = new MeterOptions
                                                                         {
                                                                             Context = ContextName,
                                                                             Name = "Total Http Error Requests",
                                                                             MeasurementUnit = Unit.Requests
                                                                         };

            public static readonly MeterOptions OverallHttpErrorRequests = new MeterOptions
                                                                            {
                                                                                Context = ContextName,
                                                                                Name = "Overall Http Error Requests",
                                                                                MeasurementUnit = Unit.Requests
                                                                            };
        }

        public static class Timers
        {
            public static readonly TimerOptions HttpRequestTransactions = new TimerOptions
                                                                          {
                                                                              Context = ContextName,
                                                                              Name = "Http Request Transactions",
                                                                              MeasurementUnit = Unit.Requests
                                                                          };

            public static readonly TimerOptions OverallHttpRequestTransactions = new TimerOptions
                                                                                 {
                                                                                     Context = ContextName,
                                                                                     Name = "Overall Http Request Transactions",
                                                                                     MeasurementUnit = Unit.Requests
                                                                                 };
        }
    }

#pragma warning restore SA1401
}