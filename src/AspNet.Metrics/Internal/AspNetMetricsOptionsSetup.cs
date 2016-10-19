// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Internal
{
    public class AspNetMetricsOptionsSetup : ConfigureOptions<AspNetMetricsOptions>
    {
        //TODO: AH - remove this or add setup config here?
        public AspNetMetricsOptionsSetup(IServiceProvider serviceProvider)
            : base(options => ConfigureMetrics(options, serviceProvider))
        {
        }

        public static void ConfigureMetrics(AspNetMetricsOptions options, 
            IServiceProvider serviceProvider)
        {
        }
    }
}