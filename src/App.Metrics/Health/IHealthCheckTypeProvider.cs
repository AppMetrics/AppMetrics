using System.Collections.Generic;
using System.Reflection;

namespace App.Metrics.Health
{
    public interface IHealthCheckTypeProvider
    {
        IEnumerable<TypeInfo> HealthCheckTypes { get; }
    }
}