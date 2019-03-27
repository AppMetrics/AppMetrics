// <copyright file="MvcRouteTemplateResolver.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
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

        public async Task<string> ResolveMatchingTemplateRouteAsync(RouteData routeData)
        {
            var templateRoute = await _routeNameResolver.ResolveMatchingTemplateRouteAsync(routeData).ConfigureAwait(false);

            if (templateRoute.IsPresent())
            {
                return templateRoute;
            }

            var attributeRouteHandler = routeData.Routers.FirstOrDefault(r => r.GetType().Name == nameof(MvcAttributeRouteHandler))
                as MvcAttributeRouteHandler;

            if (attributeRouteHandler == null || !attributeRouteHandler.Actions.Any())
            {
                return string.Empty;
            }

            if (attributeRouteHandler.Actions.Length == 1)
            {
                var singleDescriptor = attributeRouteHandler.Actions.Single();

                return ExtractRouteTemplate(routeData, singleDescriptor);
            }

            foreach (var actionDescriptor in attributeRouteHandler.Actions)
            {
                if (actionDescriptor.Properties != null && actionDescriptor.Properties.ContainsKey(MsVersionpolicyIsAppliedToken))
                {
                    return ExtractRouteTemplate(routeData, actionDescriptor);
                }
            }

            var firstDescriptor = attributeRouteHandler.Actions.First();

            return ExtractRouteTemplate(routeData, firstDescriptor);
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
    }
}