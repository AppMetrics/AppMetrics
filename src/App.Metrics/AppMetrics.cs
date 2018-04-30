// <copyright file="AppMetrics.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.ReservoirSampling.ExponentialDecay;

namespace App.Metrics
{
    /// <summary>
    /// Provides convenience methods for creating instances of <see cref="IMetrics"/> and <see cref="IMetricsBuilder"/> with pre-configured defaults.
    /// </summary>
    public static class AppMetrics
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="IMetricsBuilder" /> class with pre-configured defaults.
        /// </summary>
        /// <remarks>
        ///     The following defaults are applied to the returned <see cref="MetricsBuilder" />:
        ///     use <see cref="DefaultForwardDecayingReservoir" /> for sampling, JSON and Plain Text metric and environment
        ///     information formatters are added, the default metrics formatting is set to JSON, the default environment
        ///     information formatting is set to Plain Text, the default metrics formatting is set to JSON.
        /// </remarks>
        /// <returns>The initialized <see cref="IMetricsBuilder" />.</returns>
        public static IMetricsBuilder CreateDefaultBuilder()
        {
            var builder = new MetricsBuilder()
                .Configuration.Configure(
                    options =>
                    {
                        options.AddServerTag();
                        options.AddAppTag();
                        options.AddEnvTag();
                    })
                .OutputEnvInfo.AsPlainText()
                .OutputEnvInfo.AsJson()
                .OutputMetrics.AsJson()
                .OutputMetrics.AsPlainText()
                .SampleWith.ForwardDecaying()
                .TimeWith.StopwatchClock();

            return builder;
        }
    }
}