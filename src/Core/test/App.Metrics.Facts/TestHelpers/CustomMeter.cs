// <copyright file="CustomMeter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Meter;

namespace App.Metrics.Facts.TestHelpers
{
    public class CustomMeter : IMeter
    {
        /// <inheritdoc />
        public void Mark()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Mark(string item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Mark(MetricSetItem setItem)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Mark(MetricSetItem setItem, long amount)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Mark(long amount)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Mark(string item, long amount)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}