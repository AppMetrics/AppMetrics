// <copyright file="MediaTypeHeaderValueExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Formatters;

// ReSharper disable CheckNamespace
namespace Microsoft.Net.Http.Headers
    // ReSharper restore CheckNamespace
{
    public static class MediaTypeHeaderValueExtensions
    {
        public static MetricsMediaTypeValue ToMetricsMediaType(this MediaTypeHeaderValue mediaTypeHeaderValue)
        {
            var versionAndFormatTokens = mediaTypeHeaderValue.SubType.Value.Split('-');

            if (mediaTypeHeaderValue.Type.Value.IsMissing()
                || mediaTypeHeaderValue.SubType.Value.IsMissing()
                || versionAndFormatTokens.Length != 2)
            {
                return default;
            }

            var versionAndFormat = versionAndFormatTokens[1].Split('+');

            if (versionAndFormat.Length != 2)
            {
                return default;
            }

            return new MetricsMediaTypeValue(
                mediaTypeHeaderValue.Type.Value,
                versionAndFormatTokens[0],
                versionAndFormat[0],
                versionAndFormat[1]);
        }
    }
}
