// <copyright file="OAuthRequestMetricsRegistry.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Options;

namespace App.Metrics.Extensions.Middleware.Internal
{
    internal static class OAuthRequestMetricsRegistry
    {
#pragma warning disable SA1401
        public static string ContextName = "Application.OAuth2";

        public static class Meters
        {
            public static readonly MeterOptions RequestRate = new MeterOptions
                                                                   {
                                                                       Context = ContextName,
                                                                       Name = "Request Rate",
                                                                       MeasurementUnit = Unit.Requests
                                                                   };

            public static readonly MeterOptions ErrorRate = new MeterOptions
                                                                                        {
                                                                                            Context = ContextName,
                                                                                            Name = "Error Rate",
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
    }
#pragma warning restore SA1401
}
