namespace App.Metrics
{
    public static class Metrics
    {
        public static IMetricsRoot Instance { get; private set; }

        internal static void SetInstance(IMetricsRoot metricsRoot)
        {
            Instance = metricsRoot;
        }
    }
}