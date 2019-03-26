// <copyright file="CustomHistogram.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
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