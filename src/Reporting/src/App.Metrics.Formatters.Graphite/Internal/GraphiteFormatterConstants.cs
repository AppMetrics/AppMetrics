// <copyright file="GraphiteFormatterConstants.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Formatters.Graphite.Internal
{
    public static class GraphiteFormatterConstants
    {
        public static class GraphiteDefaults
        {
            public static readonly IGraphitePointTextWriter MetricPointTextWriter = new DefaultGraphitePointTextWriter();
        }
    }
}