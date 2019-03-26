// <copyright file="DefaultMetricsRouteNameResolver.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using App.Metrics.AspNetCore;
using Microsoft.AspNetCore.Routing.Template;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Routing
    // ReSharper restore CheckNamespace
{
    public class DefaultMetricsRouteNameResolver : IRouteNameResolver
    {
        public Task<string> ResolveMatchingTemplateRouteAsync(RouteData routeData)
        {
            var templateRoute = routeData.Routers.FirstOrDefault(r => r.GetType().Name == "Route")
                as Route;

            var controller = routeData.Values.FirstOrDefault(v => v.Key == "controller");
            var action = routeData.Values.FirstOrDefault(v => v.Key == "action");
            var version = routeData.Values.FirstOrDefault(v => v.Key == "version");

            var result = string.Empty;

            if (templateRoute != null)
            {
                result = templateRoute.ToTemplateString(
                    controller.Value as string,
                    action.Value as string,
                    version.Value == null ? string.Empty : version.Value as string);
            }

            return Task.FromResult(result);
        }
    }
}