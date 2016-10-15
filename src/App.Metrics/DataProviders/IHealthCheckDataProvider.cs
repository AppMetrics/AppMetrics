using System.Threading.Tasks;
using App.Metrics.Health;

namespace App.Metrics.DataProviders
{
    public interface IHealthCheckDataProvider
    {
        Task<HealthStatus> GetStatusAsync();
    }
}