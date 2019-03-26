// <copyright file="HealthFixture.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace Health.Extensions.DependencyInjection.Facts.Fixtures
{
    public class HealthFixture : IDisposable
    {
        public HealthFixture()
        {
            DependencyContext = DependencyContext.Load(Assembly.Load(typeof(HealthFixture).Assembly.GetName().Name));
        }

        public DependencyContext DependencyContext { get; }

        public void Dispose() { }
    }
}