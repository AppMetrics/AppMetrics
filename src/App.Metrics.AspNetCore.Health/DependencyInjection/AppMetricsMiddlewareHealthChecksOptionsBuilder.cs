// <copyright file="AppMetricsMiddlewareHealthChecksOptionsBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    internal sealed class AppMetricsMiddlewareHealthChecksOptionsBuilder : IAppMetricsMiddlewareHealthChecksOptionsBuilder
    {
        internal AppMetricsMiddlewareHealthChecksOptionsBuilder(IAppMetricsHealthChecksBuilder appMetricsChecksBuilder)
        {
            AppMetricsHealthChecksChecksBuilder = appMetricsChecksBuilder ?? throw new ArgumentNullException(nameof(appMetricsChecksBuilder));
        }

        public IAppMetricsHealthChecksBuilder AppMetricsHealthChecksChecksBuilder { get; }
    }
}