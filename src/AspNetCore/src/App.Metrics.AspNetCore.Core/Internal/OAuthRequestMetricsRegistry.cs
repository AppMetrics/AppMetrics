// <copyright file="OAuthRequestMetricsRegistry.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using App.Metrics.BucketHistogram;
using App.Metrics.Histogram;
using App.Metrics.Meter;

namespace App.Metrics.AspNetCore.Internal
{
    [ExcludeFromCodeCoverage]
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

        public static class BucketHistograms
        {
            public static readonly Func<double[], BucketHistogramOptions> PutRequestSizeHistogram = buckets => new BucketHistogramOptions
            {
                                                                                  Context = ContextName,
                                                                                  Name = "PUT Size",
                                                                                  MeasurementUnit = Unit.Bytes,
                                                                                  Buckets = buckets
                                                                              };

            public static readonly Func<double[], BucketHistogramOptions> PostRequestSizeHistogram = buckets => new BucketHistogramOptions
            {
                                                                                   Context = ContextName,
                                                                                   Name = "POST Size",
                                                                                   MeasurementUnit = Unit.Bytes,
                                                                                   Buckets = buckets
                                                                               };
        }
    }
#pragma warning restore SA1401
}
