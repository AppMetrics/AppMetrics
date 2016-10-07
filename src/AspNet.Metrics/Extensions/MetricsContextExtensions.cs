
// ReSharper disable CheckNamespace
namespace App.Metrics
// ReSharper restore CheckNamespace
{
    internal static class MetricsContextExtensions
    {
        private const string OAuth2ClientWebRequestsContextName = "Application.OAuth2Client.WebRequests";
        private const string WebApplicationRequestsContextName = "Application.WebRequests";

        public static IMetricsContext GetOAuth2ClientWebRequestsContext(this IMetricsContext context)
        {
            return context.Context(OAuth2ClientWebRequestsContextName);
        }

        public static IMetricsContext GetWebApplicationContext(this IMetricsContext context)
        {
            return context.Context(WebApplicationRequestsContextName);
        }
    }
}