using System;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Health;
using App.Metrics.Infrastructure;
using App.Metrics.Internal;

namespace HealthCheck.Samples
{
    public interface IDatabase
    {
        void Ping();
    }

    public class Database : IDatabase
    {
        public void Ping()
        {
            throw new NotImplementedException();
        }
    }

    public class DatabaseHealthCheck : App.Metrics.Health.HealthCheck
    {
        private readonly IDatabase _database;

        public DatabaseHealthCheck(IDatabase database)
            : base("DatabaseCheck")
        {
            _database = database;
        }

        protected override Task<HealthCheckResult> CheckAsync()
        {
            // exceptions will be caught and 
            // the result will be unhealthy
            _database.Ping();

            return AppMetricsTaskCache.CompletedHealthyTask;
        }
    }
}