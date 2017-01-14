// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Extensions.Reporting.InfluxDB.Client
{
    public struct LineProtocolWriteResult
    {
        public LineProtocolWriteResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public bool Success { get; }
    }
}