// <copyright file="IRouteNameResolver.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading.Tasks;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Routing
{
    // ReSharper restore CheckNamespace
    public interface IRouteNameResolver
    {
        Task<string> ResolveMatchingTemplateRouteAsync(RouteData routeData);
    }
}