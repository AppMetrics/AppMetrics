// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System;
using System.Threading.Tasks;
using App.Metrics.DependencyInjection.Internal;

namespace App.Metrics.Core
{
    public class HealthCheck
    {
        private readonly Func<Task<HealthCheckResult>> _check;

        public HealthCheck(string name, Func<Task<string>> check)
            : this(name, async () => HealthCheckResult.Healthy(await check()))
        {
        }

        public HealthCheck(string name, Func<Task<HealthCheckResult>> check)
        {
            Name = name;
            _check = check;
        }

        protected HealthCheck(string name)
        {
            Name = name;
            _check = () => AppMetricsTaskCache.CompletedHealthyTask;
        }

        public string Name { get; }

        public async Task<Result> ExecuteAsync()
        {
            try
            {
                return new Result(Name, await CheckAsync());
            }
            catch (Exception ex)
            {
                return new Result(Name, HealthCheckResult.Unhealthy(ex));
            }
        }

        protected virtual Task<HealthCheckResult> CheckAsync()
        {
            return _check();
        }

        public struct Result
        {
            public readonly HealthCheckResult Check;
            public readonly string Name;

            public Result(string name, HealthCheckResult check)
            {
                Name = name;
                Check = check;
            }
        }
    }
}