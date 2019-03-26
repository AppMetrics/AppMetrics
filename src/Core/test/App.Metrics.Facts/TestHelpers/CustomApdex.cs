// <copyright file="CustomApdex.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Apdex;

namespace App.Metrics.Facts.TestHelpers
{
    public class CustomApdex : IApdex
    {
        public long CurrentTime() { return 0; }

        public long EndRecording() { return 0; }

        public ApdexContext NewContext() { return new ApdexContext(new CustomApdex()); }

        public void Reset() { }

        public long StartRecording() { return 0; }

        public void Track(long duration) { }

        public void Track(Action action) { }

        public T Track<T>(Func<T> action) { return action(); }
    }
}