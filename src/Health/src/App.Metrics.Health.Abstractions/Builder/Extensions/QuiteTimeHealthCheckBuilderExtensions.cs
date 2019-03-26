// <copyright file="QuiteTimeHealthCheckBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public static class QuiteTimeHealthCheckBuilderExtensions
    {
        public static IHealthBuilder AddQuiteTimeCheck(
            this IHealthCheckBuilder builder,
            string name,
            Func<ValueTask<HealthCheckResult>> check,
            HealthCheck.QuiteTime quiteTime)
        {
            builder.AddCheck(new HealthCheck(name, check, quiteTime));

            return builder.Builder;
        }

        public static IHealthBuilder AddQuiteTimeCheck(
            this IHealthCheckBuilder builder,
            string name,
            Func<CancellationToken, ValueTask<HealthCheckResult>> check,
            HealthCheck.QuiteTime quiteTime)
        {
            builder.AddCheck(new HealthCheck(name, check, quiteTime));

            return builder.Builder;
        }
    }
}
