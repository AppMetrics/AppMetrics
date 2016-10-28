// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;

namespace App.Metrics
{
    public class AppMetricsHealthCheckOptions
    {
        public AppMetricsHealthCheckOptions()
        {
            IsEnabled = true;
            HealthChecks = factory => { };
        }

        public Action<IHealthCheckFactory> HealthChecks { get; set; }


        public bool IsEnabled { get; set; }
    }
}