// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using App.Metrics.Data;

namespace App.Metrics.Core
{
    public interface IMetricsDataProvider
    {
        /// <summary>
        ///     Returns the current metrics data for the context for which this provider has been created.
        /// </summary>
        Task<MetricsDataValueSource> ReadDataAsync();

        Task<MetricsDataValueSource> ReadDataAsync(IMetricsFilter overrideGlobalFilter);

        Task<MetricsContextValueSource> ReadContextAsync(string context);

        void Reset();

        void ShutdownContext(string context);
    }
}