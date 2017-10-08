// <copyright file="MetricsJsonOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json
{
    /// <summary>
    ///     Provides programmatic configuration for JSON formatting the App Metrics framework.
    /// </summary>
    public class MetricsJsonOptions
    {
        public JsonSerializerSettings SerializerSettings { get; } =
            DefaultJsonSerializerSettings.CreateSerializerSettings();
    }
}
