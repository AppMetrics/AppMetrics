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

        /// <summary>
        ///     Initializes a new instance of the <see cref="HealthCheck" /> class.
        /// </summary>
        /// <param name="name">A descriptive name for the health check.</param>
        /// <param name="check">A function returning a message that is a healthy result.</param>
        public HealthCheck(string name, Func<Task<string>> check)
            : this(name, async () => HealthCheckResult.Healthy(await check()))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HealthCheck" /> class.
        /// </summary>
        /// <param name="name">A descriptive name for the health check.</param>
        /// <param name="check">A function returning either a healthy or un-healthy result.</param>
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

        /// <summary>
        ///     A descriptive name for the health check.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Executes the health check asynchrously
        /// </summary>
        /// <returns>The <see cref="Result" /> of running the health check</returns>
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

        /// <summary>
        ///     Represents the result of running a <see cref="HealthCheck" />
        /// </summary>
        public struct Result
        {
            public readonly HealthCheckResult Check;
            public readonly string Name;

            /// <summary>
            ///     Initializes a new instance of the <see cref="Result" /> struct.
            /// </summary>
            /// <param name="name">A descriptive name for the health check</param>
            /// <param name="check">The result of executing a health check.</param>
            public Result(string name, HealthCheckResult check)
            {
                Name = name;
                Check = check;
            }
        }
    }
}