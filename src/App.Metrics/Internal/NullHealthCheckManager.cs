// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using App.Metrics.Core;

namespace App.Metrics.Internal
{
    internal sealed class NullHealthCheckManager : IHealthStatusProvider
    {
        public Task<HealthStatus> ReadStatusAsync()
        {
            return Task.FromResult(new HealthStatus());
        }
    }
}