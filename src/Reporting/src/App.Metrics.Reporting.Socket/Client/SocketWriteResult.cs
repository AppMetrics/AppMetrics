// <copyright file="SocketWriteResult.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Reporting.Socket.Client
{
    public struct SocketWriteResult
    {
        public SocketWriteResult(bool success)
        {
            Success = success;
            ErrorMessage = null;
        }

        public SocketWriteResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public bool Success { get; }
    }
}
