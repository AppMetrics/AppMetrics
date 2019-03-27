// <copyright file="DefaultJsonSerializerSettings.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace App.Metrics.Health.Reporting.Slack.Internal
{
    internal static class DefaultJsonSerializerSettings
    {
        private const int DefaultMaxDepth = 32;

        private static readonly DefaultContractResolver DefaultContractResolver = new DefaultContractResolver
                                                                                  {
                                                                                      NamingStrategy = new SnakeCaseNamingStrategy()
                                                                                  };

        private static readonly Lazy<JsonSerializerSettings> Lazy =
            new Lazy<JsonSerializerSettings>(
                () =>
                {
                    var settings = new JsonSerializerSettings
                                   {
                                       DateParseHandling = DateParseHandling.DateTimeOffset,
                                       MaxDepth = DefaultMaxDepth,
                                       ContractResolver = DefaultContractResolver,
                                       NullValueHandling = NullValueHandling.Ignore,
                                       MissingMemberHandling = MissingMemberHandling.Ignore,
                                       Formatting = Formatting.Indented,
                                       TypeNameHandling = TypeNameHandling.None
                                   };

                    settings.Converters.Add(new StringEnumConverter());

                    return settings;
                });

        public static JsonSerializerSettings Instance => Lazy.Value;
    }
}
