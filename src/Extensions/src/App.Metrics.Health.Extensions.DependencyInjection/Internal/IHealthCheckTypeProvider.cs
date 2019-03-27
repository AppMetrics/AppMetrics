// <copyright file="IHealthCheckTypeProvider.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Reflection;

namespace App.Metrics.Health.Extensions.DependencyInjection.Internal
{
    internal interface IHealthCheckTypeProvider
    {
        IEnumerable<TypeInfo> HealthCheckTypes { get; }
    }
}