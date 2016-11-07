// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using App.Metrics.Data;
using App.Metrics.Utils;

namespace App.Metrics
{
    /// <summary>
    ///     A provider capable of returning the current values for a set of metrics
    /// </summary>
    public interface IMetricsDataManager : IHideObjectMembers
    {
        /// <summary>
        ///     Returns the current metrics data for the context for which this provider has been created.
        /// </summary>
        Task<MetricsDataValueSource> GetMetricsDataAsync();
    }
}