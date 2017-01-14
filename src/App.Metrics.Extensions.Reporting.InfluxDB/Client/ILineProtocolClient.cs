// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Extensions.Reporting.InfluxDB.Client
{
    public interface ILineProtocolClient
    {
        Task<LineProtocolWriteResult> WriteAsync(
            LineProtocolPayload payload,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}