// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading;

namespace App.Metrics.Reporters
{
    public interface IScheduledReporter
    {
        void Dispose();

        void Start(CancellationToken token);
    }
}