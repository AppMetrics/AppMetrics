// <copyright file="MetricsResourceFilter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.DependencyInjection.Internal;
using App.Metrics.Extensions.Middleware.DependencyInjection.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Mvc.Filters
{
    // ReSharper restore CheckNamespace
    public class MetricsResourceFilter : IAsyncResourceFilter
    {
        private readonly IRouteNameResolver _routeNameResolver;
        private ILogger _logger;

        public MetricsResourceFilter(IRouteNameResolver routeNameResolver)
        {
            _routeNameResolver = routeNameResolver ?? throw new ArgumentNullException(nameof(routeNameResolver));
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            // Verify if AddMetrics and AddAspNetMetrics was done before calling UseMetrics
            // We use the MetricsMarkerService and AspNetMetricsMarkerService to make sure if all the services were added.
            MetricsServicesHelper.ThrowIfMetricsNotRegistered(context.HttpContext.RequestServices);
            AspNetMetricsServicesHelper.ThrowIfMetricsNotRegistered(context.HttpContext.RequestServices);

            EnsureServices(context.HttpContext);

            var templateRoute = await _routeNameResolver.ResolveMatchingTemplateRouteAsync(context.RouteData);

            if (!string.IsNullOrEmpty(templateRoute))
            {
                context.HttpContext.AddMetricsCurrentRouteName(templateRoute);
            }

            await next.Invoke();
        }

        private void EnsureServices(HttpContext context)
        {
            if (_logger != null)
            {
                return;
            }

            var factory = context.RequestServices.GetRequiredService<ILoggerFactory>();
            _logger = factory.CreateLogger<MetricsResourceFilter>();
        }
    }
}