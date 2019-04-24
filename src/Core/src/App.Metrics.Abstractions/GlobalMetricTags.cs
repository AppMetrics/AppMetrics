// <copyright file="GlobalMetricTags.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace App.Metrics
{
    public class GlobalMetricTags : Dictionary<string, string>
    {
        public GlobalMetricTags(Dictionary<string, string> tags)
            : base(tags) { }

        public GlobalMetricTags() { }
    }
}