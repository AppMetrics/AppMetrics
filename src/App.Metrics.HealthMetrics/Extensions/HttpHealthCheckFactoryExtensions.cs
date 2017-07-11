// <copyright file="HttpHealthCheckFactoryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
using App.Metrics.Internal;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    // TODO: 2.0.0 - requires middleware, should it live here or be refactored?

    public static class HttpHealthCheckFactoryExtensions
    {
#pragma warning disable SA1008, SA1009
        private static readonly ApdexOptions ApdexOptions = new ApdexOptions
                                                            {
                                                                Context = "Application.HttpRequests",
                                                                Name = "Apdex"
                                                            };

        /// <summary>
        /// Registers the Apdex Score of overall Http Requests (response times).
        /// </summary>
        /// <param name="registry">The health check registry</param>
        /// <param name="metrics">The <see cref="IMetrics"/> instance providing the apdex</param>
        /// <param name="apdexTSeconds">The Apdex T seconds value.</param>
        /// <returns>the health check registry</returns>
        public static IHealthCheckRegistry AddOveralWebRequestsApdexCheck(
            this IHealthCheckRegistry registry,
            IMetrics metrics,
            double apdexTSeconds = AppMetricsReservoirSamplingConstants.DefaultApdexTSeconds)
        {
            ApdexOptions.ApdexTSeconds = apdexTSeconds;

            return registry.AddMetricCheck(
                "Apdex Score",
                metrics,
                ApdexOptions,
                passing: apdex => (message: $"Satisfied. Score: {apdex.Score}", result: apdex.Score >= 0.75),
                warning: apdex => (message: $"Tolerating. Score: {apdex.Score}", result: apdex.Score >= 0.5 && apdex.Score < 0.75),
                failing: apdex => (message: $"Frustrating. Score: {apdex.Score}", result: apdex.Score < 0.5));
        }
#pragma warning restore SA1008, SA1009
    }
}