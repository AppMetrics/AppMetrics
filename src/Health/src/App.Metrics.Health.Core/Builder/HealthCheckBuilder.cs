// <copyright file="HealthCheckBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health.Builder
{
    public class HealthCheckBuilder : IHealthCheckBuilder
    {
        private readonly Action<HealthCheck> _healthCheck;

        internal HealthCheckBuilder(
            IHealthBuilder healthBuilder,
            Action<HealthCheck> healthCheck)
        {
            Builder = healthBuilder ?? throw new ArgumentNullException(nameof(healthBuilder));
            _healthCheck = healthCheck ?? throw new ArgumentNullException(nameof(healthCheck));
        }

        /// <inheritdoc />
        public IHealthBuilder Builder { get; }

        /// <inheritdoc />
        public IHealthBuilder AddChecks(IEnumerable<HealthCheck> checks)
        {
            Register(checks);

            return Builder;
        }

        /// <inheritdoc />
        public IHealthBuilder AddCheck<THealthCheck>(THealthCheck check)
            where THealthCheck : HealthCheck
        {
            Register(check);

            return Builder;
        }

        /// <inheritdoc />
        public IHealthBuilder AddCheck<THealthCheck>()
            where THealthCheck : HealthCheck, new()
        {
            var check = new THealthCheck();

            Register(check);

            return Builder;
        }

        /// <inheritdoc />
        public IHealthBuilder AddCheck(string name, Func<ValueTask<HealthCheckResult>> check)
        {
            Register(new HealthCheck(name, check));

            return Builder;
        }

        /// <inheritdoc />
        public IHealthBuilder AddCheck(string name, Func<CancellationToken, ValueTask<HealthCheckResult>> check)
        {
            Register(new HealthCheck(name, check));

            return Builder;
        }

        internal void Register(IEnumerable<HealthCheck> healthChecks)
        {
            foreach (var check in healthChecks)
            {
                _healthCheck(check);
            }
        }

        internal void Register(HealthCheck healthCheck)
        {
            _healthCheck(healthCheck);
        }
    }
}
