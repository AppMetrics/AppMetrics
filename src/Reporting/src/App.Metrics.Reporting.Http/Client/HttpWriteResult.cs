// <copyright file="HttpWriteResult.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Reporting.Http.Client
{
    public struct HttpWriteResult
    {
        public HttpWriteResult(bool success)
        {
            Success = success;
            ErrorMessage = null;
        }

        public HttpWriteResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public bool Success { get; }
    }
}