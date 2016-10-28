// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


namespace App.Metrics.Json
{
    public class JsonMetric
    {
        private string[] tags = MetricTags.None.Tags;

        public string Name { get; set; }

        public string[] Tags
        {
            get { return tags; }
            set { tags = value ?? new string[0]; }
        }

        public string Unit { get; set; }
    }
}