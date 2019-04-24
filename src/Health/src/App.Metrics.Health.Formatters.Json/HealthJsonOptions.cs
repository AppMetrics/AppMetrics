// <copyright file="HealthJsonOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace App.Metrics.Health.Formatters.Json
{
    public class HealthJsonOptions
    {
        public JsonSerializerSettings SerializerSettings { get; } =
            DefaultJsonSerializerSettings.CreateSerializerSettings();
    }
}