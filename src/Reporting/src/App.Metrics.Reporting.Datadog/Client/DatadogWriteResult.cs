// <copyright file="DatadogWriteResult.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Reporting.Datadog.Client
{
    public struct DatadogWriteResult
    {
        public DatadogWriteResult(bool success)
        {
            Success = success;
            ErrorMessage = null;
        }

        public DatadogWriteResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public bool Success { get; }

        public static readonly DatadogWriteResult SuccessResult = new DatadogWriteResult(true);
    }
}