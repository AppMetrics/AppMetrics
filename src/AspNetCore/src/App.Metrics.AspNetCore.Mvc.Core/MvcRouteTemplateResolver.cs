// <copyright file="MvcRouteTemplateResolver.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.AspNetCore;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Mvc.Internal
    // ReSharper restore CheckNamespace
{
    public class MvcRouteTemplateResolver : IRouteNameResolver
    {
        private const string ApiVersionToken = "{version:apiversion}";
        private const string MsVersionpolicyIsAppliedToken = "MS_VersionPolicyIsApplied";
        private const string VersionRouteDataToken = "version";
        private readonly IRouteNameResolver _routeNameResolver;

        public MvcRouteTemplateResolver()
            : this(new DefaultMetricsRouteNameResolver()) { }

        private MvcRouteTemplateResolver(IRouteNameResolver routeNameResolver)
        {
            _routeNameResolver = routeNameResolver ?? throw new ArgumentNullException(nameof(routeNameResolver));
        }

#if NETCOREAPP
        public Task<string> ResolveMatchingTemplateRouteAsync(RouteData routeData)
        {
            return Task.FromResult(string.Empty);
        }
#else
        public async Task<string> ResolveMatchingTemplateRouteAsync(RouteData routeData)
        {
            var templateRoute = await _routeNameResolver.ResolveMatchingTemplateRouteAsync(routeData).ConfigureAwait(false);

            if (templateRoute.IsPresent())
            {
                return templateRoute;
            }

            var actions = GetActionDescriptors(routeData.Routers);

            if (actions == null || !actions.Any())
            {
                return string.Empty;
            }

            if (actions.Length == 1)
            {
                var singleDescriptor = actions.Single();

                return ExtractRouteTemplate(routeData, singleDescriptor);
            }

            foreach (var actionDescriptor in actions)
            {
                if (actionDescriptor.Properties != null && actionDescriptor.Properties.ContainsKey(MsVersionpolicyIsAppliedToken))
                {
                    return ExtractRouteTemplate(routeData, actionDescriptor);
                }
            }

            var firstDescriptor = actions.First();

            return ExtractRouteTemplate(routeData, firstDescriptor);
        }

        private ActionDescriptor[] GetActionDescriptors(IList<IRouter> routers)
        {
            try
            {
                // It's hack to catch TypeLoadException
                Func<IList<IRouter>, ActionDescriptor[]> func = routersList =>
                {
                    var attributeRouteHandler = routersList.FirstOrDefault(r => r.GetType().Name == nameof(MvcAttributeRouteHandler))
                        as MvcAttributeRouteHandler;
                
                    return attributeRouteHandler?.Actions;
                };
                
                return func(routers);
            }
            catch (TypeLoadException)
            {
                return null;
            }
        }

        private static string ExtractRouteTemplate(RouteData routeData, ActionDescriptor actionDescriptor)
        {
            var routeTemplate = actionDescriptor.AttributeRouteInfo?.Template.ToLower() ?? string.Empty;

            if (actionDescriptor.Properties != null && actionDescriptor.Properties.ContainsKey(MsVersionpolicyIsAppliedToken)
                && routeData.Values.ContainsKey(VersionRouteDataToken))
            {
                routeTemplate = routeTemplate.Replace(ApiVersionToken, routeData.Values[VersionRouteDataToken].ToString());
            }

            return routeTemplate;
        }
#endif
    }
}