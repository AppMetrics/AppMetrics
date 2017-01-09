// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Extensions.Owin.DependencyInjection.Options;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Internal;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Owin.Middleware
{
    public class MetricsEndpointTextEndpointMiddleware : AppMetricsMiddleware<OwinMetricsOptions>
    {
        private readonly DefaultReportGenerator _reportGenerator;

        public MetricsEndpointTextEndpointMiddleware(OwinMetricsOptions owinOptions,
            ILoggerFactory loggerFactory,
            IMetrics metrics)
            : base(owinOptions, loggerFactory, metrics)
        {
            _reportGenerator = new DefaultReportGenerator(loggerFactory);
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var requestPath = environment["owin.RequestPath"] as string;

            if (Options.MetricsTextEndpointEnabled && Options.MetricsTextEndpoint.IsPresent() && Options.MetricsTextEndpoint == requestPath)
            {
                Logger.MiddlewareExecuting(GetType());

                var stringReporter = new StringReporter();
                //TODO: AH - confirm cancellation token is correct
                var cancellationToken = (CancellationToken)environment["owin.CallCancelled"];
                await _reportGenerator.GenerateAsync(stringReporter, Metrics, cancellationToken);

                await WriteResponseAsync(environment, stringReporter.Result, "text/plain");

                Logger.MiddlewareExecuted(GetType());

                return;
            }

            await Next(environment);
        }
    }
}