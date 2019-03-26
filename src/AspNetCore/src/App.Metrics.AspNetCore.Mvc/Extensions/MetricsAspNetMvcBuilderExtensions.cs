// <copyright file="MetricsAspNetMvcBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Mvc
    // ReSharper restore CheckNamespace
{
    public static class MetricsAspNetMvcBuilderExtensions
    {
        /// <summary>
        ///     Adds App Metrics MVC options for example adds a resource filter to provide app metrics with routes for tagging metrics.
        /// </summary>
        /// <param name="mvcBuilder">The <see cref="T:Microsoft.Extensions.DependencyInjection.IMvcBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="T:Microsoft.Extensions.DependencyInjection.IMvcBuilder" /> cannot be null
        /// </exception>
        public static IMvcBuilder AddMetrics(this IMvcBuilder mvcBuilder)
        {
            if (mvcBuilder == null)
            {
                throw new ArgumentNullException(nameof(mvcBuilder));
            }

            mvcBuilder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<MvcOptions>, MetricsMvcOptionsSetup>());

            return mvcBuilder;
        }
    }
}