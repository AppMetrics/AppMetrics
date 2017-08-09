// <copyright file="EnvironmentInfoSerializer.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Infrastructure;

namespace App.Metrics.Serialization
{
    /// <summary>
    ///     Serializes <see cref="EnvironmentInfo" /> into the different formats.
    /// </summary>
    public class EnvironmentInfoSerializer
    {
        /// <summary>
        ///     Serializes the specified <see cref="EnvironmentInfo" /> and writes the environment information using the specified
        ///     <see cref="IEnvInfoWriter" />.
        /// </summary>
        /// <param name="writer">The <see cref="IMetricSnapshotWriter" /> used to write the metrics snapshot.</param>
        /// <param name="envInfo">The <see cref="EnvironmentInfo" /> to serilize.</param>
        public void Serialize(IEnvInfoWriter writer, EnvironmentInfo envInfo) { writer.Write(envInfo); }
    }
}