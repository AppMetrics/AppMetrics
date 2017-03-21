// <copyright file="NullApdex.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Apdex;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core.Internal;

namespace App.Metrics.Registry.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal struct NullApdex : IApdex
    {
        public long CurrentTime() { return 0; }

        public long EndRecording() { return 0; }

        public ApdexContext NewContext() { return new ApdexContext(this); }

        public void Reset() { }

        public long StartRecording() { return 0; }

        public void Track(long duration) { }

        public void Track(Action action) { }

        public T Track<T>(Func<T> action) { return action(); }
    }
}