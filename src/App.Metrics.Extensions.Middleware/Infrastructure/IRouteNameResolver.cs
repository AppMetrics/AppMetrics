// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace App.Metrics.Extensions.Middleware.Infrastructure
{
    public interface IRouteNameResolver
    {
        Task<string> ResolveMatchingTemplateRoute(RouteData routeData);
    }
}