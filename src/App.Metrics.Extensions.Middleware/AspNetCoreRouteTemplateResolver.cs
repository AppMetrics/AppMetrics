// <copyright file="AspNetCoreRouteTemplateResolver.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Template;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Routing
{
    // ReSharper restore CheckNamespace
    public class AspNetCoreRouteTemplateResolver : IRouteNameResolver
    {
        public Task<string> ResolveMatchingTemplateRouteAsync(RouteData routeData)
        {
            var templateRoute = routeData.Routers
                                         .FirstOrDefault(r => r.GetType().Name == "Route")
                as Route;

            if (templateRoute == null)
            {
                return Task.FromResult(string.Empty);
            }

            var controller = routeData.Values.FirstOrDefault(v => v.Key == "controller");
            var action = routeData.Values.FirstOrDefault(v => v.Key == "action");

            var result = templateRoute.ToTemplateString(controller.Value as string, action.Value as string);

            return Task.FromResult(result);
        }
    }
}