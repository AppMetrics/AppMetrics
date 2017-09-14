// <copyright file="IRunMetricsReports.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Reporting
{
    public interface IRunMetricsReports
    {
        IEnumerable<Task> RunAllAsync(CancellationToken cancellationToken = default);
    }
}