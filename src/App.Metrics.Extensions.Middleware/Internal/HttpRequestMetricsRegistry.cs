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
            public static readonly CounterOptions ActiveRequestCount = new CounterOptions
                                                                   {
                                                                       Context = ContextName,
                                                                       Name = "Active",
                                                                       MeasurementUnit = Unit.Custom("Active Requests")
                                                                   };

            public static readonly CounterOptions TotalErrorRequestCount = new CounterOptions
                                                                       {
                                                                           Context = ContextName,
                                                                           Name = "Errors",
                                                                           ResetOnReporting = true,
                                                                           MeasurementUnit = Unit.Requests
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
            public static readonly HistogramOptions PutRequestSizeHistogram = new HistogramOptions
                                                                            {
                                                                                Context = ContextName,
                                                                                Name = "PUT Size",
                                                                                MeasurementUnit = Unit.Bytes
                                                                            };

            public static readonly HistogramOptions PostRequestSizeHistogram = new HistogramOptions
                                                                            {
                                                                                Context = ContextName,
                                                                                Name = "POST Size",
                                                                                MeasurementUnit = Unit.Bytes
                                                                            };
        }

        public static class Meters
        {
            public static readonly MeterOptions EndpointErrorRequestRate = new MeterOptions
                                                                             {
                                                                                 Context = ContextName,
                                                                                 Name = "Error Rate Per Endpoint",
                                                                                 MeasurementUnit = Unit.Requests
                                                                             };

            public static readonly MeterOptions EndpointErrorRequestPerStatusCodeRate = new MeterOptions
                                                                           {
                                                                               Context = ContextName,
                                                                               Name = "Error Rate Per Endpoint And Status Code",
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
    }

#pragma warning restore SA1401
}