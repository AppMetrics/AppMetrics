// <copyright file="ProtoFormatter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;

namespace App.Metrics.Formatters.Prometheus.Internal
{
    internal static class ProtoFormatter
    {
        public static byte[] Format(IEnumerable<MetricFamily> metrics)
        {
            using (var memoryStream = new MemoryStream())
            {
                Format(memoryStream, metrics);
                return memoryStream.ToArray();
            }
        }

        public static void Format(Stream destination, IEnumerable<MetricFamily> metrics)
        {
            var metricFamilys = metrics.ToArray();
            foreach (var metricFamily in metricFamilys)
            {
                Serializer.SerializeWithLengthPrefix(destination, metricFamily, PrefixStyle.Base128, 0);
            }
        }
    }
}