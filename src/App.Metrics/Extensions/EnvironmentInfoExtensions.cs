using System.Collections.Generic;

// ReSharper disable CheckNamespace

namespace App.Metrics.Infrastructure
// ReSharper restore CheckNamespace
{
    public static class EnvironmentInfoExtensions
    {
        public static MetricTags ToTags(this EnvironmentInfo environmentInfo)
        {
            return new MetricTags(new Dictionary<string, string>
            {
                { "version", environmentInfo.EntryAssemblyVersion },
                { "host", environmentInfo.MachineName },
                { "ip_adress", environmentInfo.IpAddress }
            });
        }
    }
}