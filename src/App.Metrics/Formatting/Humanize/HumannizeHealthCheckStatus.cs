using App.Metrics.Internal;

// ReSharper disable CheckNamespace
namespace App.Metrics.Core
// ReSharper restore CheckNamespace
{
    public static class HumannizeHealthCheckStatus
    {
        public static string Hummanize(this HealthCheckStatus status)
        {
            return Constants.Health.HealthStatusDisplay[status];
        }
    }
}