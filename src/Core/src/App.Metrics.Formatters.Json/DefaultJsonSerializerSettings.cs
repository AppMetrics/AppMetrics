// <copyright file="DefaultJsonSerializerSettings.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Text.Json;
using App.Metrics.Formatters.Json.Converters;

namespace App.Metrics.Formatters.Json
{
    public static class DefaultJsonSerializerSettings
    {
        private const int DefaultMaxDepth = 32;

        /// <summary>
        ///     Creates default <see cref="JsonSerializerOptions" />.
        /// </summary>
        /// <returns>Default <see cref="JsonSerializerOptions" />.</returns>
        public static JsonSerializerOptions CreateSerializerOptions()
        {
            var settings = new JsonSerializerOptions
                           {
                               IgnoreNullValues = true,
                               WriteIndented = true,
                               MaxDepth = DefaultMaxDepth,
                               PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                               DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
                           };

            settings.Converters.Add(new MetricDataConverter());
            settings.Converters.Add(new EnvironmentInfoConverter());
            settings.Converters.Add(new DictionaryConverter());

            return settings;
        }
    }
}