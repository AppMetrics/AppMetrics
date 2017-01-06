// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;

namespace App.Metrics
{
    public class GlobalMetricTags : Dictionary<string, string>
    {
        public GlobalMetricTags(Dictionary<string, string> tags)
            : base(tags)
        {
        }

        public GlobalMetricTags()
        {
        }
    }
}