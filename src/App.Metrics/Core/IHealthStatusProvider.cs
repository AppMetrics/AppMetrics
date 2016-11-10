using System.Threading.Tasks;

namespace App.Metrics.Core
{
    public interface IHealthStatusProvider
    {
        Task<HealthStatus> ReadStatusAsync();
    }
}