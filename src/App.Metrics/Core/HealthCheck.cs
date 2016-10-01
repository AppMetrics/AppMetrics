using System;

namespace App.Metrics.Core
{
    public class HealthCheck
    {
        private readonly Func<HealthCheckResult> _check;

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
            Name = name;
            _check = check;
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
                return new Result(Name, Check());
            }
            catch (Exception x)
            {
                return new Result(Name, HealthCheckResult.Unhealthy(x));
            }
        }

        protected virtual HealthCheckResult Check()
        {
            return _check();
        }

        public struct Result
        {
            public readonly HealthCheckResult Check;
            public readonly string Name;

            public Result(string name, HealthCheckResult check)
            {
                Name = name;
                Check = check;
            }
        }
    }
}