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
    public interface IApdexMetric : IApdex, IMetricValueProvider<ApdexValue>, IDisposable
    {
    }
}