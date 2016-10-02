using System;
using System.Collections.Generic;
using App.Metrics;
using App.Metrics.Core;

namespace AspNet.Metrics.Facts
{
    public class TestHealthCheck : HealthCheck
    {
        public TestHealthCheck() : base("Test Health Check")
        {
        }

        protected override HealthCheckResult Check()
        {
            return HealthCheckResult.Healthy("OK");
        }
    }
}
