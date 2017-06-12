// <copyright file="IHealthCheckTypeProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Reflection;

namespace App.Metrics.Health.Internal
{
    internal interface IHealthCheckTypeProvider
    {
        IEnumerable<TypeInfo> HealthCheckTypes { get; }
    }
}