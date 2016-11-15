// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using AspNet.Metrics.Infrastructure;

// ReSharper disable CheckNamespace

namespace Microsoft.AspNetCore.Mvc
// ReSharper restore CheckNamespace
{
    public static class MvcOptionsExtensions
    {
        public static MvcOptions AddMetricsResourceFilter(this MvcOptions options)
        {
            options.Filters.Add(new MetricsResourceFilter(new DefaultRouteTemplateResolver()));

            return options;
        }

        public static MvcOptions AddMetricsResourceFilter(this MvcOptions options, IRouteNameResolver routeNameResolver)
        {
            options.Filters.Add(new MetricsResourceFilter(routeNameResolver));

            return options;
        }
    }
}