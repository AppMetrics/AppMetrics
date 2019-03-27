// <copyright file="CacheHealthCheckBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public static class CacheHealthCheckBuilderExtensions
    {
        public static IHealthBuilder AddCachedCheck(
            this IHealthCheckBuilder builder,
            string name,
            Func<ValueTask<HealthCheckResult>> check,
            TimeSpan cacheDuration)
        {
            builder.AddCheck(new HealthCheck(name, check, cacheDuration));

            return builder.Builder;
        }

        public static IHealthBuilder AddCachedCheck(
            this IHealthCheckBuilder builder,
            string name,
            Func<CancellationToken, ValueTask<HealthCheckResult>> check,
            TimeSpan cacheDuration)
        {
            builder.AddCheck(new HealthCheck(name, check, cacheDuration));

            return builder.Builder;
        }
    }
}
