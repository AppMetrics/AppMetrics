// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Reflection;

namespace App.Metrics.Health.Abstractions
{
    internal interface IHealthCheckTypeProvider
    {
        IEnumerable<TypeInfo> HealthCheckTypes { get; }
    }
}