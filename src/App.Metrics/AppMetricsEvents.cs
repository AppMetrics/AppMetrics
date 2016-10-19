// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using App.Metrics.Internal;

namespace App.Metrics
{
    public class AppMetricsEvents : IAppMetricsEvents
    {
        //public Func<Exception, Task> OnError { get; set; } = context => AppMetricsTaskCache.CompletedTask;

        //public virtual Task Error(Exception context) => OnError(context);
    }
}