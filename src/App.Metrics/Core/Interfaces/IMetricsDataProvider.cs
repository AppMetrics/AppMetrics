// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using App.Metrics.Data;

namespace App.Metrics.Core.Interfaces
{
    public interface IMetricsDataProvider
    {
        Task<MetricsContextValueSource> ReadContextAsync(string context);

        /// <summary>
        ///     Returns the current metrics data for the context for which this provider has been created.
        /// </summary>
        Task<MetricsDataValueSource> ReadDataAsync();

        Task<MetricsDataValueSource> ReadDataAsync(IMetricsFilter overrideGlobalFilter);

        void Reset();

        void ShutdownContext(string context);
    }
}