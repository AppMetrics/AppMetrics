// ReSharper disable CheckNamespace

namespace Metrics
// ReSharper restore CheckNamespace
{
    internal static class MetricsContextExtensions
    {
        private const string OAuth2ClientWebRequestsContextName = "Application.OAuth2Client.WebRequests";
        private const string WebApplicationRequestsContextName = "Application.WebRequests";

        public static MetricsContext GetOAuth2ClientWebRequestsContext(this MetricsContext context)
        {
            return context.Context(OAuth2ClientWebRequestsContextName);
        }

        public static MetricsContext GetWebApplicationContext(this MetricsContext context)
        {
            return context.Context(WebApplicationRequestsContextName);
        }
    }
}