// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Data;

namespace App.Metrics.Internal
{
    internal class DefaultMetricsDataManager : IMetricsDataManager
    {
        private IMetricsRegistry _registry;
        private bool _isDisabled = false;

        public DefaultMetricsDataManager(IMetricsRegistry registry)
        {
            _registry = registry;
        }

        public Task<MetricsDataValueSource> GetAsync()
        {
            return _registry.GetDataAsync();
        }

        public async Task<MetricsDataValueSource> GetAsync(IMetricsFilter filter)
        {
            var data = await GetAsync();

            return data.Filter(filter);
        }

        public async Task<MetricsDataGroupValueSource> GetByGroupAsync(string groupName)
        {
            var data = await GetAsync();

            var groupFilter = new DefaultMetricsFilter()
                .WhereGroup(groupName);

            var groupData = data.Filter(groupFilter);

            return groupData.Groups.Single();
        }


        public void Reset()
        {
            _registry.Clear();
        }

        public void ShutdownGroup(string groupName)
        {
            _registry.RemoveGroup(groupName);
        }

        public void DisableMetrics()
        {
            if (_isDisabled)
            {
                return;
            }

            _isDisabled = true;

            _registry.Clear();
            Interlocked.Exchange(ref _registry, new NullMetricsRegistry());
        }
    }
}