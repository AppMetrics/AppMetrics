// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using App.Metrics.Health;

namespace App.Metrics.DataProviders
{
    public sealed class NullHealthCheckManager : IHealthCheckManager
    {
        public Task<HealthStatus> GetStatusAsync()
        {
            return Task.FromResult(new HealthStatus());
        }
    }
}