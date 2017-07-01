// <copyright file="AppMetricsMiddlewareOptionsBuilder.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    internal sealed class AppMetricsMiddlewareOptionsBuilder : IAppMetricsMiddlewareOptionsBuilder
    {
        internal AppMetricsMiddlewareOptionsBuilder(IAppMetricsBuilder appMetricsBuilder)
        {
            AppMetricsBuilder = appMetricsBuilder ?? throw new ArgumentNullException(nameof(appMetricsBuilder));
        }

        public IAppMetricsBuilder AppMetricsBuilder { get; }
    }
}