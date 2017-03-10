// <copyright file="IMetricsEnvironment.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Infrastructure
{
    public interface IMetricsEnvironment
    {
        string ApplicationName { get; }

        string ApplicationVersion { get; }

        string RuntimeFramework { get; }

        string RuntimeFrameworkVersion { get; }
    }
}