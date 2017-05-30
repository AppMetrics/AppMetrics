using App.Metrics.Core.Options;

namespace App.Metrics.Sandbox
{
    public static class SandboxMetricsRegistry
    {
        public const string ContextName = "Sandbox";

        public static readonly TimerOptions DatabaseTimer = new TimerOptions
                                                            {
                                                                Context = ContextName,
                                                                Name = "Database Call"                                                                
                                                            };
    }
}