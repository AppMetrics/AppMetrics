// <copyright file="HttpHealthCheckFactoryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
using App.Metrics.Health;
using App.Metrics.Internal;
using App.Metrics.Middleware.DependencyInjection.Options;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class HttpHealthCheckFactoryExtensions
    {
#pragma warning disable SA1008, SA1009
        private static readonly ApdexOptions ApdexOptions = new ApdexOptions
                                                            {
                                                                Context = "Application.HttpRequests",
                                                                Name = "Apdex"
                                                            };

        /// <summary>
        /// Registers the Apdex Score of overall Http Requests (response times). Requires <see cref="AppMetricsMiddlewareOptions.ApdexTrackingEnabled"/> to be <value>true</value>.
        /// </summary>
        /// <param name="factory">The health check factory</param>
        /// <param name="apdexTSeconds">The Apdex T seconds value <see cref="AppMetricsMiddlewareOptions.ApdexTSeconds"/></param>
        /// <returns>the health check factory</returns>
        public static IHealthCheckFactory RegisterOveralWebRequestsApdexCheck(
            this IHealthCheckFactory factory,
            double apdexTSeconds = ReservoirSamplingConstants.DefaultApdexTSeconds)
        {
            ApdexOptions.ApdexTSeconds = apdexTSeconds;

            return factory.RegisterMetricCheck(
                "Apdex Score",
                ApdexOptions,
                passing: apdex => (message: $"Satisfied. Score: {apdex.Score}", result: apdex.Score >= 0.75),
                warning: apdex => (message: $"Tolerating. Score: {apdex.Score}", result: apdex.Score >= 0.5 && apdex.Score < 0.75),
                failing: apdex => (message: $"Frustrating. Score: {apdex.Score}", result: apdex.Score < 0.5));
        }
#pragma warning restore SA1008, SA1009
    }
}