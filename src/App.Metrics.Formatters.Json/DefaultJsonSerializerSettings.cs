// <copyright file="DefaultJsonSerializerSettings.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Formatters.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace App.Metrics.Formatters.Json
{
    public static class DefaultJsonSerializerSettings
    {
        private const int DefaultMaxDepth = 32;

        private static readonly DefaultContractResolver SharedContractResolver = new DefaultContractResolver
                                                                                 {
                                                                                     NamingStrategy = new CamelCaseNamingStrategy()
                                                                                 };

        /// <summary>
        ///     Creates default <see cref="JsonSerializerSettings" />.
        /// </summary>
        /// <returns>Default <see cref="JsonSerializerSettings" />.</returns>
        public static JsonSerializerSettings CreateSerializerSettings()
        {
            var settings = new JsonSerializerSettings
                           {
                               ContractResolver = SharedContractResolver,
                               NullValueHandling = NullValueHandling.Ignore,
                               MissingMemberHandling = MissingMemberHandling.Ignore,
                               Formatting = Formatting.Indented,
                               MaxDepth = DefaultMaxDepth,
                               TypeNameHandling = TypeNameHandling.None
                           };

            settings.Converters.Add(new MetricDataConverter());
            settings.Converters.Add(new EnvironmentInfoConverter());

            return settings;
        }
    }
}