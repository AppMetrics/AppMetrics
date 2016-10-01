using System;

namespace App.Metrics.Core
{
    public class HealthCheck
    {
        private readonly Func<HealthCheckResult> check;

        public HealthCheck(string name, Action check)
            : this(name, () =>
            {
                check();
                return string.Empty;
            })
        {
        }

        public HealthCheck(string name, Func<string> check)
            : this(name, () => HealthCheckResult.Healthy(check()))
        {
        }

        public HealthCheck(string name, Func<HealthCheckResult> check)
        {
            this.Name = name;
            this.check = check;
        }

        protected HealthCheck(string name)
            : this(name, () => { })
        {
        }

        public string Name { get; }

        public Result Execute()
        {
            try
            {
                return new Result(this.Name, this.Check());
            }
            catch (Exception x)
            {
                return new Result(this.Name, HealthCheckResult.Unhealthy(x));
            }
        }

        protected virtual HealthCheckResult Check()
        {
            return this.check();
        }

        public struct Result
        {
            public readonly HealthCheckResult Check;
            public readonly string Name;

            public Result(string name, HealthCheckResult check)
            {
                this.Name = name;
                this.Check = check;
            }
        }
    }
}