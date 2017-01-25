// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health
{
    public class HealthCheck
    {
        private readonly Func<CancellationToken, Task<HealthCheckResult>> _check;

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

            Func<CancellationToken, Task<HealthCheckResult>> checkWithToken = token => check();

            _check = checkWithToken;
        }

        protected HealthCheck(string name)
        {
            Name = name;
            _check = token => AppMetricsTaskCache.CompletedHealthyTask;
        }

        /// <summary>
        ///     Gets the descriptive name for the health check.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        ///     Executes the health check asynchrously
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     The <see cref="Result" /> of running the health check
        /// </returns>
        public async Task<Result> ExecuteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                return new Result(Name, await CheckAsync(cancellationToken));
            }
            catch (Exception ex)
            {
                return new Result(Name, HealthCheckResult.Unhealthy(ex));
            }
        }

        protected virtual Task<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return _check(cancellationToken);
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