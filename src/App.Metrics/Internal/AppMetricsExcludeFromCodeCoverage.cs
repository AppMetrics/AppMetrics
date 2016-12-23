using System;

namespace App.Metrics.Internal
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    internal class AppMetricsExcludeFromCodeCoverage : Attribute
    {
    }
}