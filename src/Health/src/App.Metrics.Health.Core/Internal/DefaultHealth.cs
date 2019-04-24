// <copyright file="DefaultHealth.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Health.Internal
{
    public class DefaultHealth : IHealth
    {
        public DefaultHealth(IEnumerable<HealthCheck> checks)
        {
            Checks = checks ?? Enumerable.Empty<HealthCheck>();
        }

        /// <inheritdoc />
        public IEnumerable<HealthCheck> Checks { get; }
    }
}
