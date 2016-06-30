using Microsoft.AspNetCore.Http;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Routing
// ReSharper restore CheckNamespace
{
    public static class MetricsContextExtensions
    {
        private static readonly string MetricsCurrentRouteName = "__Mertics.CurrentRouteName__";

        public static void AddMetricsCurrentRouteName(this HttpContext context, string metricName)
        {
            context.Items.Add(MetricsCurrentRouteName, metricName);
        }

        public static string GetMetricsCurrentRouteName(this HttpContext context)
        {
            return context.Request.Method + " " + context.Items[MetricsCurrentRouteName];
        }

        public static bool HasMetricsCurrentRouteName(this HttpContext context)
        {
            return context.Items.ContainsKey(MetricsCurrentRouteName);
        }
    }
}