// <copyright file="HealthFixture.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Health.Internal;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Health.Formatters.Ascii.Facts.Fixtures
{
    public class HealthFixture : IDisposable
    {
        private readonly ILoggerFactory _loggerFactory = new LoggerFactory();

        public HealthFixture()
        {
            HealthCheckRegistry = new HealthCheckRegistry();
            var healthStatusProvider = new DefaultHealthProvider(
                _loggerFactory.CreateLogger<DefaultHealthProvider>(),
                HealthCheckRegistry);
            Health = healthStatusProvider;
        }

        public IHealthCheckRegistry HealthCheckRegistry { get; }

        public IProvideHealth Health { get; }

        public void Dispose() { }
    }
}