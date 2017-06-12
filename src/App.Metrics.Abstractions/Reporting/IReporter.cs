// <copyright file="IReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;

namespace App.Metrics.Reporting
{
    public interface IReporter : IDisposable
    {
        void RunReports(IMetrics context, CancellationToken token);
    }
}