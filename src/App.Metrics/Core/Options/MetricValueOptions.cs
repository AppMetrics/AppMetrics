// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace App.Metrics.Core.Options
{
    public abstract class MetricValueOptions
    {
        protected MetricValueOptions()
        {
            Tags = MetricTags.None;
            MeasurementUnit = Unit.None;
        }

        public string Context { get; set; }

        public Unit MeasurementUnit { get; set; }

        public string Name { get; set; }

        public MetricTags Tags { get; set; }
    }
}