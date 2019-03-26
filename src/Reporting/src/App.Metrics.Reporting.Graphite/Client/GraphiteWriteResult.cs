// <copyright file="GraphiteWriteResult.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Reporting.Graphite.Client
{
    public struct GraphiteWriteResult
    {
        public GraphiteWriteResult(bool success)
        {
            Success = success;
            ErrorMessage = null;
        }

        public GraphiteWriteResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public bool Success { get; }

        public static readonly GraphiteWriteResult SuccessResult = new GraphiteWriteResult(true);
    }
}