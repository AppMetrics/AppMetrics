// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Apdex;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core.Abstractions;

namespace App.Metrics.Abstractions.Metrics
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