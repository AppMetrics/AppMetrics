using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace App.Metrics.Facts
{
    public interface IDatabase
    {
        void Ping();
    }

    public class Database : IDatabase
    {
        public void Ping()
        {
        }
    }

    public class DatabaseHealthCheck : HealthCheck
    {
        private readonly IDatabase _database;

        public DatabaseHealthCheck(IDatabase database)
            : base("DatabaseCheck")
        {
            _database = database;
        }

        protected override Task<HealthCheckResult> CheckAsync(CancellationToken token = default(CancellationToken))
        {
            // exceptions will be caught and 
            // the result will be unhealthy
            _database.Ping();

            return AppMetricsTaskCache.CompletedHealthyTask;
        }
    }
}