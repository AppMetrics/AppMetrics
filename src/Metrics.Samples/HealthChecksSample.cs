
using System;
using App.Metrics;
using App.Metrics.Core;

namespace Metrics.Samples
{
    public class HealthChecksSample
    {
        public interface IDatabase { void Ping(); }

        public class DatabaseHealthCheck : HealthCheck
        {
            private readonly IDatabase database;
            public DatabaseHealthCheck(IDatabase database)
                : base("DatabaseCheck")
            {
                this.database = database;
                HealthChecks.RegisterHealthCheck(this);
            }

            protected override HealthCheckResult Check()
            {
                // exceptions will be caught and 
                // the result will be unhealthy

                if (database == null)
                {
                    return HealthCheckResult.Unhealthy();
                }

                this.database.Ping();

                return HealthCheckResult.Healthy();
            }
        }

        public static void RegisterHealthChecks()
        {
            new DatabaseHealthCheck(null);

            HealthChecks.RegisterHealthCheck("DatabaseConnected", () =>
            {
                CheckDbIsConnected();
                return "Database Connection OK";
            });

            HealthChecks.RegisterHealthCheck("DiskSpace", () =>
            {
                int freeDiskSpace = GetFreeDiskSpace();

                if (freeDiskSpace <= 512)
                {
                    return HealthCheckResult.Unhealthy("Not enough disk space: {0}", freeDiskSpace);
                }
                else
                {
                    return HealthCheckResult.Unhealthy("Disk space ok: {0}", freeDiskSpace);
                }
            });

            //HealthChecks.RegisterHealthCheck("SampleOperation", () => SampleOperation());
            //HealthChecks.RegisterHealthCheck("AggregateSampleOperation", () => AggregateSampleOperation());
        }

        private static void SampleOperation()
        {
            try
            {
                try
                {
                    throw new InvalidCastException("Sample error");
                }
                catch (Exception x)
                {
                    throw new FormatException("Another Error", x);
                }
            }
            catch (Exception x)
            {
                throw new InvalidOperationException("operation went south", x);
            }
        }

        private static void AggregateSampleOperation()
        {
            try
            {
                SampleOperation();
            }
            catch (Exception x1)
            {
                try
                {
                    SampleOperation();
                }
                catch (Exception x2)
                {
                    throw new AggregateException(new[] { x1, x2 });
                }
            }
        }

        public static void CheckDbIsConnected()
        {
        }

        public static int GetFreeDiskSpace()
        {
            return 1024;
        }

    }
}
