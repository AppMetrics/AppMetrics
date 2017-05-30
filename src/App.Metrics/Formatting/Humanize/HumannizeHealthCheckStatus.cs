// <copyright file="HumannizeHealthCheckStatus.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Internal;
using App.Metrics.Health;

// ReSharper disable CheckNamespace
namespace App.Metrics.Core
{
    // TODO: Remove in 2.0.0
    // ReSharper restore CheckNamespace
    [Obsolete("Replaced with formatting packages which can be used with the Report Runner")]
    public static class HumannizeHealthCheckStatus
    {
        public static string Hummanize(this HealthCheckStatus status) { return Constants.Health.HealthStatusDisplay[status]; }
    }
}