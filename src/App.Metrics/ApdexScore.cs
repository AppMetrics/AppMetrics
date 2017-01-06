// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace App.Metrics
{
    public sealed class ApdexScore : Metric
    {
        public int Frustrating { get; set; }

        public int SampleSize { get; set; }

        public int Satisfied { get; set; }

        public double Score { get; set; }

        public int Tolerating { get; set; }
    }
}