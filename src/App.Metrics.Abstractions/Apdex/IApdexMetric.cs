// <copyright file="IApdexMetric.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Apdex
{
    /// <summary>
    ///     Provides access to a adpex metric implementation.
    ///     <seealso>
    ///         <cref>App.Metrics.Core.Interfaces.IApdex</cref>
    ///     </seealso>
    ///     <seealso>
    ///         <cref>App.Metrics.Data.Interfaces.IMetricValueProvider{ApdexValue}</cref>
    ///     </seealso>
    ///     <seealso>
    ///         <cref>System.IDisposable</cref>
    ///     </seealso>
    /// </summary>
    public interface IApdexMetric : IApdex, IMetricValueProvider<ApdexValue>, IDisposable
    {
    }
}