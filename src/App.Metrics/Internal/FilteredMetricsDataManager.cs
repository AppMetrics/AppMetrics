// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using App.Metrics.Data;

namespace App.Metrics.Internal
{
    internal sealed class FilteredMetricsDataManager : IMetricsDataManager
    {
        private readonly IMetricsFilter _filter;
        private readonly IMetricsDataManager _manager;

        public FilteredMetricsDataManager(IMetricsDataManager manager, IMetricsFilter filter)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            _manager = manager;
            _filter = filter;
        }

        public async Task<MetricsDataValueSource> GetAsync()
        {
            var data = await _manager.GetAsync();
            return data.Filter(_filter);
        }

        public void Reset()
        {
            //TODO: AH - filter metrics via other means?
            _manager.Reset();
        }

        public void ShutdownGroup(string groupName)
        {
            _manager.ShutdownGroup(groupName);
        }
    }
}