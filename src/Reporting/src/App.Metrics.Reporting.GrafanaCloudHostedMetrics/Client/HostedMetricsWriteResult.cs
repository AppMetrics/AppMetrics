// <copyright file="HostedMetricsWriteResult.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Reporting.GrafanaCloudHostedMetrics.Client
{
    public struct HostedMetricsWriteResult
    {
        public HostedMetricsWriteResult(bool success)
        {
            Success = success;
            ErrorMessage = null;
        }

        public HostedMetricsWriteResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public bool Success { get; }

        public static readonly HostedMetricsWriteResult SuccessResult = new HostedMetricsWriteResult(true);
    }
}