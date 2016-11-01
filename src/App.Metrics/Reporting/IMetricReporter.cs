// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using App.Metrics.Infrastructure;
using App.Metrics.MetricData;
using App.Metrics.Utils;

namespace App.Metrics.Reporting
{
    public interface IMetricReporter : IHideObjectMembers, IDisposable
    {
        void EndMetricTypeReport(Type metricType);

        void EndReport(IMetricsContext metricsContext);

        void ReportMetric<T>(string name, MetricValueSource<T> valueSource);

        void ReportEnvironment(EnvironmentInfo environmentInfo);

        void ReportHealth(IEnumerable<HealthCheck.Result> healthyChecks, IEnumerable<HealthCheck.Result> unhealthyChecks);

        void StartMetricTypeReport(Type metricType);

        void StartReport(IMetricsContext metricsContext);
    }
}