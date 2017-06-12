// <copyright file="IApdexMetric.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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