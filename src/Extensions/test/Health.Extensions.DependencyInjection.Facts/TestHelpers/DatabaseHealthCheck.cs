// <copyright file="DatabaseHealthCheck.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace Health.Extensions.DependencyInjection.Facts.TestHelpers
{
    public class DatabaseHealthCheck : HealthCheck
    {
        private readonly IDatabase _database;

        public DatabaseHealthCheck(IDatabase database)
            : base("DatabaseCheck")
        {
            _database = database;
        }

        protected override ValueTask<HealthCheckResult> CheckAsync(CancellationToken token = default)
        {
            _database.Open();

            return new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy());
        }
    }
}