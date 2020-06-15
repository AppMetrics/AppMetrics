// <copyright file="DatadogOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Reporting.Datadog
{
    public class DatadogOptions
    {
        public DatadogOptions(Uri baseUri, string apiKey)
        {
            BaseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        public DatadogOptions()
        {
        }

        public Uri BaseUri { get; set; }

        public string ApiKey { get; set; }
    }
}
