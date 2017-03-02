// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Histogram.Abstractions;

namespace App.Metrics.Facts
{
    public class CustomHistogram : IHistogram
    {
        /// <inheritdoc />
        public void Reset() { throw new NotImplementedException(); }

        /// <inheritdoc />
        public void Update(long value, string userValue) { throw new NotImplementedException(); }

        /// <inheritdoc />
        public void Update(long value) { throw new NotImplementedException(); }
    }
}