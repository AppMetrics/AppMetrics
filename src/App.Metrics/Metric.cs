// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics
{
    public abstract class Metric
    {
        private MetricTags _tags = new MetricTags();

        public string Name { get; set; }

        public MetricTags Tags
        {
            get { return _tags; }
            set { _tags = value ?? new MetricTags(); }
        }

        public string Unit { get; set; }
    }
}