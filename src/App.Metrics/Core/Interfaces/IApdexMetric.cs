// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Data;
using App.Metrics.Data.Interfaces;

namespace App.Metrics.Core.Interfaces
{
    /// <summary>
    ///     Provides access to a adpex metric implementation e.g. <see cref="ApdexMetric" />
    /// <seealso>
    ///     <cref>App.Metrics.Core.Interfaces.IApdex</cref>
    /// </seealso>
    /// <seealso>
    ///     <cref>App.Metrics.Data.Interfaces.IMetricValueProvider{ApdexValue}</cref>
    /// </seealso>
    /// <seealso>
    ///     <cref>System.IDisposable</cref>
    /// </seealso>
    /// </summary>
    public interface IApdexMetric : IApdex, IMetricValueProvider<ApdexValue>, IDisposable
    {
    }
}