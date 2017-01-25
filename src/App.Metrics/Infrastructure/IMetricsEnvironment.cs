// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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