// <copyright file="LineProtocolWriteResult.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Reporting.InfluxDB.Client
{
    public struct LineProtocolWriteResult
    {
        public LineProtocolWriteResult(bool success)
        {
            Success = success;
            ErrorMessage = null;
        }

        public LineProtocolWriteResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public bool Success { get; }
    }
}