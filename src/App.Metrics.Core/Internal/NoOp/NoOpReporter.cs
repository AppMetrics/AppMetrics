// <copyright file="NoOpReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using App.Metrics.Reporting;

namespace App.Metrics.Core.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    internal sealed class NoOpReporter : IReporter
    {
        /// <inheritdoc />
        public void Dispose() { }

        public void RunReports(IMetrics context, CancellationToken token) { }
    }
}