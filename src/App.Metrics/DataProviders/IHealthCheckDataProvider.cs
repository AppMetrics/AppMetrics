using System.Threading.Tasks;

namespace App.Metrics.DataProviders
{
    public interface IHealthCheckDataProvider
    {
        Task<HealthStatus> GetStatusAsync();
    }
}