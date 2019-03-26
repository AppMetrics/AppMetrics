// <copyright file="MetricsMvcCoreOptionsSetup.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Options;

namespace App.Metrics.AspNetCore.Mvc.Core
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class MetricsMvcCoreOptionsSetup : IConfigureOptions<MvcOptions>
        // ReSharper restore ClassNeverInstantiated.Global
    {
        /// <inheritdoc />
        public void Configure(MvcOptions options)
        {
            if (!options.Filters.OfType<MetricsResourceFilter>().Any())
            {
                options.Filters.Add(new MetricsResourceFilter(new MvcRouteTemplateResolver()));
            }
        }
    }
}