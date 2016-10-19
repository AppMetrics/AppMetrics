using System.Collections.Generic;
using System.Text.RegularExpressions;
using AspNet.Metrics.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace AspNet.Metrics
{
    public class AspNetMetricsOptions
    {
        public AspNetMetricsOptions()
        {
            IgnoredRequestPatterns = new List<Regex>();
            RouteNameResolver = new DefaultRouteTemplateResolver();
            HealthEndpoint = new PathString("/health");
            MetricsEndpoint = new PathString("/metrics");
            MetricsTextEndpoint = new PathString("/metrics-text");
            PingEndpoint = new PathString("/ping");

            HealthEnabled = true;
            MetricsEnabled = true;
            MetricsTextEnabled = true;
            PingEnabled = true;
            OAuth2TrackingEnabled = true;

            IgnoredRequestPatterns.Add(new Regex(@"\.(jpg|gif|css|js|png|woff|ttf|txt|eot|svg)$"));
            IgnoredRequestPatterns.Add(new Regex("(?i)^swagger"));
            IgnoredRequestPatterns.Add(new Regex("(?i)^metrics"));
            IgnoredRequestPatterns.Add(new Regex("(?i)^metrics-text"));
            IgnoredRequestPatterns.Add(new Regex("(?i)^metrics-visual"));
            IgnoredRequestPatterns.Add(new Regex("(?i)^health"));
            IgnoredRequestPatterns.Add(new Regex("(?i)^ping"));
            IgnoredRequestPatterns.Add(new Regex("(?i)^favicon.ico"));
        }

        public bool OAuth2TrackingEnabled { get; set; }

        public bool HealthEnabled { get; set; }

        public PathString HealthEndpoint { get; set; }

        public IList<Regex> IgnoredRequestPatterns { get; }

        public bool MetricsEnabled { get; set; }

        public PathString MetricsEndpoint { get; set; }

        public bool MetricsTextEnabled { get; set; }

        public PathString MetricsTextEndpoint { get; set; }

        public bool PingEnabled { get; set; }

        public PathString PingEndpoint { get; set; }

        public IRouteNameResolver RouteNameResolver { get; set; }
    }
}