using System;

namespace AspNet.Metrics
{
    /// <summary>
    ///     Indicates that the type and any derived types that this attribute is applied to
    ///     is not considered a health check by the default health check discovery mechanism.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class NonHealthCheckAttribute : Attribute
    {
    }
}