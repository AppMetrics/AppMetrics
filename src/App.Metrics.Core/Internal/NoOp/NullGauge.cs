// <copyright file="NullGauge.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using App.Metrics.Gauge;

namespace App.Metrics.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    public struct NullGauge : IGauge
    {
        /// <inheritdoc />
        public void Reset()
        {
        }

        /// <inheritdoc />
        public void SetValue(double value)
        {
        }
    }
}