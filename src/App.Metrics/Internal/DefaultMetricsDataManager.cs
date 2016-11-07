// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


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