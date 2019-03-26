// <copyright file="MetricsResourceFilter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.AspNetCore;
using App.Metrics.Extensions.DependencyInjection.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Mvc.Filters
    // ReSharper restore CheckNamespace
{
    public class MetricsResourceFilter : IAsyncResourceFilter
    {
        private const string ControllerRouteKey = "controller";
        private const string ActionRouteKey = "action";
        private const string VersionRouteKey = "version";
        private const string DefaultVersionRouteKey = "{version:apiVersion}";
        private readonly IRouteNameResolver _routeNameResolver;
        private ILogger _logger;

        public MetricsResourceFilter(IRouteNameResolver routeNameResolver)
        {
            _routeNameResolver = routeNameResolver ?? throw new ArgumentNullException(nameof(routeNameResolver));
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            // Verify if AppMetrics was configured
            AppMetricsServicesHelper.ThrowIfMetricsNotRegistered(context.HttpContext.RequestServices);

            EnsureServices(context.HttpContext);

            var templateRoute = await _routeNameResolver.ResolveMatchingTemplateRouteAsync(context.RouteData);

            if (!string.IsNullOrEmpty(templateRoute))
            {
                context.HttpContext.AddMetricsCurrentRouteName(templateRoute);
            }
            else
            {
                templateRoute = context.ActionDescriptor?.AttributeRouteInfo?.Template;

                if (!string.IsNullOrEmpty(templateRoute))
                {
                    if (!string.IsNullOrEmpty(templateRoute) && templateRoute.Contains(DefaultVersionRouteKey))
                    {
                        if (context.RouteData.Values.ContainsKey(VersionRouteKey))
                        {
                            templateRoute = templateRoute.Replace(DefaultVersionRouteKey, context.RouteData.Values.Single(x => x.Key == VersionRouteKey).Value.ToString());
                        }

                        context.HttpContext.AddMetricsCurrentRouteName(templateRoute.ToLowerInvariant());
                    }

                    context.HttpContext.AddMetricsCurrentRouteName(templateRoute.ToLowerInvariant());
                }
                else
                {
                    if (context.RouteData == null || !context.RouteData.Values.Any())
                    {
                        templateRoute = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                    }
                    else
                    {
                        var controller = context.RouteData.Values.FirstOrDefault(v => v.Key == ControllerRouteKey);
                        var action = context.RouteData.Values.FirstOrDefault(v => v.Key == ActionRouteKey);
                        var version = context.RouteData.Values.FirstOrDefault(v => v.Key == VersionRouteKey);

                        templateRoute = (version.Value == null ? string.Empty : $"VER-{version.Value} ") + $"{controller.Value}/{action.Value}";
                    }

                    context.HttpContext.AddMetricsCurrentRouteName(templateRoute.ToLowerInvariant());
                }
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