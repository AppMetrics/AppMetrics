// <copyright file="HealthCheck.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics
{
    public class HealthCheck
    {
        private readonly Func<CancellationToken, ValueTask<HealthCheckResult>> _check;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HealthCheck" /> class.
        /// </summary>
        /// <param name="name">A descriptive name for the health check.</param>
        /// <param name="check">A function returning either a healthy or un-healthy result.</param>
        public HealthCheck(string name, Func<ValueTask<HealthCheckResult>> check)
        {
            Name = name;

            ValueTask<HealthCheckResult> CheckWithToken(CancellationToken token) => check();

            _check = CheckWithToken;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HealthCheck" /> class.
        /// </summary>
        /// <param name="name">A descriptive name for the health check.</param>
        /// <param name="check">A function returning either a healthy or un-healthy result.</param>
        public HealthCheck(string name, Func<CancellationToken, ValueTask<HealthCheckResult>> check)
        {
            Name = name;

            ValueTask<HealthCheckResult> CheckWithToken(CancellationToken token) => check(token);

            _check = CheckWithToken;
        }

        protected HealthCheck(string name)
        {
            Name = name;
            _check = token => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy());
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
        public async ValueTask<Result> ExecuteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var checkResult = await CheckAsync(cancellationToken);
                return new Result(Name, checkResult);
            }
            catch (Exception ex)
            {
                return new Result(Name, HealthCheckResult.Unhealthy(ex));
            }
        }

        protected virtual ValueTask<HealthCheckResult> CheckAsync(CancellationToken cancellationToken = default(CancellationToken))
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