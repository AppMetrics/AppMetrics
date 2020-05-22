// <copyright file="ContextualMetricTags.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics
{
    public class ContextualMetricTagProviders : Dictionary<string, Func<string>>
    {
        public ContextualMetricTagProviders(Dictionary<string, Func<string>> tagProviders)
            : base(tagProviders) { }

        public ContextualMetricTagProviders() { }

        public Dictionary<string, string> ComputeTagValues()
        {
            return this.ToDictionary(pair => pair.Key, pair => pair.Value());
        }
    }
}