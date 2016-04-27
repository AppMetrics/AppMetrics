using System.Collections.Generic;
using System.Text.RegularExpressions;
using AspNet.Metrics.Infrastructure;
using Microsoft.AspNet.Http;

namespace AspNet.Metrics
{
    public class MetricsOptions
    {
        public MetricsOptions()
        {
            IgnoredRequestPatterns = new List<Regex>();
            RouteNameResolver = new DefaultRouteTemplateResolver();
        }

        public bool HealthEnabled { get; set; }

        public PathString HealthEndpoint { get; set; }

        public IList<Regex> IgnoredRequestPatterns { get; }

        public bool MetricsEnabled { get; set; }

        public PathString MetricsEndpoint { get; set; }

        public bool MetricsTextEnabled { get; set; }

        public PathString MetricsTextEndpoint { get; set; }

        public bool MetricsVisualisationEnabled { get; set; }

        public PathString MetricsVisualizationEndpoint { get; set; }

        public bool PingEnabled { get; set; }

        public PathString PingEndpoint { get; set; }

        public IRouteNameResolver RouteNameResolver { get; set; }
    }
}