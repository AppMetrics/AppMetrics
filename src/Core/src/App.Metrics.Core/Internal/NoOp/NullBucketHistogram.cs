// <copyright file="NullHistogram.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using App.Metrics.BucketHistogram;

namespace App.Metrics.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    public struct NullBucketHistogram : IBucketHistogram
    {
        public void Reset() { }

        public void Update(long value, string userValue) { }

        public void Update(long value) { }
    }
}