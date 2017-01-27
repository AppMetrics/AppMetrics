// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core.Options;

namespace App.Metrics.Extensions.Middleware.Internal
{
    internal static class OAuth2MetricsRegistry
    {
#pragma warning disable SA1401

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
#pragma warning restore SA1401
    }
}