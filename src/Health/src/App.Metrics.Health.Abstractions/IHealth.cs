// <copyright file="IHealth.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics.Health
{
    public interface IHealth
    {
        IEnumerable<HealthCheck> Checks { get; }
    }
}
