// <copyright file="IGraphitePointTextWriter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.IO;
using System.Threading.Tasks;
using App.Metrics.Formatters.Graphite.Internal;

namespace App.Metrics.Formatters.Graphite
{
    public interface IGraphitePointTextWriter
    {
        ValueTask WriteAsync(TextWriter textWriter, GraphitePoint point, bool writeTimestamp = true);
    }
}