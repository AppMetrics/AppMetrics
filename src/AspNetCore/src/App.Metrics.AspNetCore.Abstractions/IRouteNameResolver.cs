// <copyright file="IRouteNameResolver.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace App.Metrics.AspNetCore
{
    public interface IRouteNameResolver
    {
        Task<string> ResolveMatchingTemplateRouteAsync(RouteData routeData);
    }
}