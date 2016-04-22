using System;

namespace AspNet.Metrics
{
    /// <summary>
    ///     Indicates that the type and any derived types that this attribute is applied to
    ///     are considered a health check by the default health check discovery mechanism, unless
    ///     <see cref="NonHealthCheckAttribute" /> is applied to any type in the hierarchy.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HealthCheckAttribute : Attribute
    {
    }
}