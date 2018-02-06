// <copyright file="NullHistogram.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using App.Metrics.Histogram;

namespace App.Metrics.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    public struct NullHistogram : IHistogram
    {
        public void Reset() { }

        public void Update(long value, string userValue) { }

        public void Update(long value) { }
    }
}