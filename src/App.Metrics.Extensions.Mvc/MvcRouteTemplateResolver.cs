// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Mvc.Internal
{
    // ReSharper restore CheckNamespace
    public class MvcRouteTemplateResolver : IRouteNameResolver
    {
        private readonly IRouteNameResolver _routeNameResolver;

        public MvcRouteTemplateResolver()
            : this(new AspNetCoreRouteTemplateResolver()) { }

        public MvcRouteTemplateResolver(IRouteNameResolver routeNameResolver)
        {
            if (routeNameResolver == null)
            {
                throw new ArgumentNullException(nameof(routeNameResolver));
            }

            _routeNameResolver = routeNameResolver;
        }

        public async Task<string> ResolveMatchingTemplateRouteAsync(RouteData routeData)
        {
            var templateRoute = await _routeNameResolver.ResolveMatchingTemplateRouteAsync(routeData).ConfigureAwait(false);

            if (templateRoute.IsPresent())
            {
                return templateRoute;
            }

            var attributeRouteHandler = routeData.Routers
                                                 .FirstOrDefault(r => r.GetType().Name == "MvcAttributeRouteHandler")
                as MvcAttributeRouteHandler;

            if (attributeRouteHandler == null)
            {
                return string.Empty;
            }

            var actionDescriptor = attributeRouteHandler.Actions.FirstOrDefault();
            var result = actionDescriptor?.AttributeRouteInfo?.Template.ToLower() ?? string.Empty;

            return result;
        }
    }
}