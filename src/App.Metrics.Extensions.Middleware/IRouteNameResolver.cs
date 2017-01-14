// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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