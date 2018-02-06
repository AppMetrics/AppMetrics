// <copyright file="CustomHistogram.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Histogram;

namespace App.Metrics.Facts.TestHelpers
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