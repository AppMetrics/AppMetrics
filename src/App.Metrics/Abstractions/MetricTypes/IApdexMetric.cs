// <copyright file="IApdexMetric.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Apdex;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core.Abstractions;

// ReSharper disable CheckNamespace
namespace App.Metrics.Abstractions.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Provides access to a adpex metric implementation e.g. <see cref="DefaultApdexMetric" />
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