// <copyright file="DatabaseHealthCheck.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Internal;

namespace App.Metrics.Facts
{
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
            // exceptions will be caught and the result will be unhealthy
            _database.Ping();

            return TaskCache.HealthCheckResultHealthyCompletedTask;
        }
    }
}