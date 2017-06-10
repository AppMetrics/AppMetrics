// <copyright file="CustomHistogram.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Histogram.Abstractions;

namespace App.Metrics.Facts
{
    public class CustomHistogram : IHistogram
    {
        /// <inheritdoc />
        public void Reset()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Update(long value, string userValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Update(long value)
        {
            throw new NotImplementedException();
        }
    }
}