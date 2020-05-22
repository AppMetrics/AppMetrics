// <copyright file="EnvironmentInfoSerializer.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Infrastructure;
using System.Threading.Tasks;

namespace App.Metrics.Serialization
{
    /// <summary>
    ///     Serializes <see cref="EnvironmentInfo" /> into the different formats.
    /// </summary>
    public sealed class EnvironmentInfoSerializer
    {
        /// <summary>
        ///     Serializes the specified <see cref="EnvironmentInfo" /> and writes the environment information using the specified
        ///     <see cref="IEnvInfoWriter" />.
        /// </summary>
        /// <param name="writer">The <see cref="IMetricSnapshotWriter" /> used to write the metrics snapshot.</param>
        /// <param name="envInfo">The <see cref="EnvironmentInfo" /> to serilize.</param>
        public async ValueTask Serialize(IEnvInfoWriter writer, EnvironmentInfo envInfo) { await writer.Write(envInfo); }
    }
}