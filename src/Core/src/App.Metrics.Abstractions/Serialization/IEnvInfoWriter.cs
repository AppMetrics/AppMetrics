// <copyright file="IEnvInfoWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;

namespace App.Metrics.Serialization
{
    public interface IEnvInfoWriter : IAsyncDisposable, IDisposable
    {
        /// <summary>
        /// Writes the specified <see cref="EnvironmentInfo"/>.
        /// </summary>
        /// <param name="envInfo">The environment information to write.</param>
        ValueTask Write(EnvironmentInfo envInfo);
    }
}
