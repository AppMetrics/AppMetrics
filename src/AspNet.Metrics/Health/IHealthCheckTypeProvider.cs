using System.Collections.Generic;
using System.Reflection;

namespace AspNet.Metrics.Health
{
    public interface IHealthCheckTypeProvider
    {
        IEnumerable<TypeInfo> HealthCheckTypes { get; }
    }
}