// <copyright file="TestHelperExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App.Metrics.Health.Formatters.Json.Facts.Helpers
{
    public enum HealthStatusSamples
    {
        Valid,
        NullHealthy,
        NullUnhealthy
    }

    public static class TestHelperExtensions
    {
        private static readonly Dictionary<HealthStatusSamples, string> HealthStatusFileSampleMapping = new Dictionary<HealthStatusSamples, string>
                                                                                                        {
                                                                                                            {
                                                                                                                HealthStatusSamples.Valid,
                                                                                                                "healthstatus"
                                                                                                            },
                                                                                                            {
                                                                                                                HealthStatusSamples.NullHealthy,
                                                                                                                "healthstatus_null_healthy"
                                                                                                            },
                                                                                                            {
                                                                                                                HealthStatusSamples.NullUnhealthy,
                                                                                                                "healthstatus_null_unhealthy"
                                                                                                            }
                                                                                                        };

        public static JToken ParseAsJson(this string json)
        {
            return JToken.Parse(json, new JsonLoadSettings { LineInfoHandling = LineInfoHandling.Ignore, CommentHandling = CommentHandling.Ignore });
        }

        public static JToken SampleJson(this HealthStatusSamples sample) { return sample.ExtractHealthStatusSampleFromResourceFile(); }

        private static JToken ExtractHealthStatusSampleFromResourceFile(this HealthStatusSamples sample)
        {
            return ExtractJsonFromEmbeddedResource(HealthStatusFileSampleMapping[sample]);
        }

        private static JToken ExtractJsonFromEmbeddedResource(string key)
        {
            var assemblyName = new AssemblyName("App.Metrics.Health.Formatters.Json.Facts");
            using (
                var fileStream =
                    Assembly.Load(assemblyName).GetManifestResourceStream($"App.Metrics.Health.Formatters.Json.Facts.JsonFiles.{key}.json"))
            {
                if (fileStream == null)
                {
                    return null;
                }

                using (var textReader = new StreamReader(fileStream))
                {
                    using (var jsonReader = new JsonTextReader(textReader))
                    {
                        return JToken.ReadFrom(jsonReader);
                    }
                }
            }
        }
    }
}