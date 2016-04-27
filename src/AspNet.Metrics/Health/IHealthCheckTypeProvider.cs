using System.Collections.Generic;
using System.Reflection;

namespace AspNet.Metrics.Health
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHealthCheckTypeProvider
    {
        IEnumerable<TypeInfo> HealthCheckTypes { get; }
    }
}