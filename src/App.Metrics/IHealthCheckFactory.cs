// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Utils;

namespace App.Metrics
{
    public interface IHealthCheckFactory : IDisposable, IHideObjectMembers
    {
        void Register(string name, Func<Task<string>> check);

        void Register(string name, Func<Task<HealthCheckResult>> check);

        IReadOnlyDictionary<string, HealthCheck> Checks { get; }
    }
}