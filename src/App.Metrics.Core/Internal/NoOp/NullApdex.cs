// <copyright file="NullApdex.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using App.Metrics.Apdex;

namespace App.Metrics.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    public struct NullApdex : IApdex
    {
        public long CurrentTime() { return 0; }

        public long EndRecording() { return 0; }

        public ApdexContext NewContext() { return new ApdexContext(this); }

        public void Reset() { }

        public long StartRecording() { return 0; }

        public void Track(long duration) { }

        public void Track(Action action) { action(); }

        public T Track<T>(Func<T> action) { return action(); }
    }
}