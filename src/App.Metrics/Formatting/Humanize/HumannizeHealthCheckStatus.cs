// <copyright file="HumannizeHealthCheckStatus.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Internal;
using App.Metrics.Health;

// ReSharper disable CheckNamespace
namespace App.Metrics.Core
{
    // ReSharper restore CheckNamespace
    public static class HumannizeHealthCheckStatus
    {
        public static string Hummanize(this HealthCheckStatus status) { return Constants.Health.HealthStatusDisplay[status]; }
    }
}