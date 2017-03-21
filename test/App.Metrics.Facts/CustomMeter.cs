// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Meter.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Facts
{
    public class CustomMeter : IMeter
    {
        /// <inheritdoc />
        public void Mark() { throw new NotImplementedException(); }

        /// <inheritdoc />
        public void Mark(string item) { throw new NotImplementedException(); }

        /// <inheritdoc />
        public void Mark(MetricSetItem setItem) { throw new NotImplementedException(); }

        /// <inheritdoc />
        public void Mark(MetricSetItem setItem, long amount) { throw new NotImplementedException(); }

        /// <inheritdoc />
        public void Mark(long amount) { throw new NotImplementedException(); }

        /// <inheritdoc />
        public void Mark(string item, long amount) { throw new NotImplementedException(); }

        /// <inheritdoc />
        public void Reset() { throw new NotImplementedException(); }
    }
}