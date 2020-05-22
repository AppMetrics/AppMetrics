// <copyright file="ILineProtocolPoint.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.IO;
using System.Threading.Tasks;

namespace App.Metrics.Formatters.InfluxDB.Internal
{
    /// <summary>
    /// Defines a point (a ligne), which can be written in Line Protocol format.
    /// </summary>
    internal interface ILineProtocolPoint
    {
        /// <summary>
        /// Write this point as a line protocol item.
        /// </summary>
        /// <param name="textWriter">Text writer to write the line to.</param>
        /// <param name="writeTimestamp">
        /// <c>true</c> to let the point write the timestamp by itself, <c>false</c> to not write the timestamp at the end of the row.
        /// You will have to write the timestamp by yourself of let the server receive the line and use its own timer as a timestamp.
        /// </param>
        ValueTask WriteAsync(TextWriter textWriter, bool writeTimestamp = true);
    }
}
