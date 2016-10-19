// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Json
{
    public sealed class JsonObject
    {
        public JsonObject(IEnumerable<JsonProperty> properties)
        {
            Properties = properties;
        }

        public IEnumerable<JsonProperty> Properties { get; }

        public string AsJson(bool indented = true, int indent = 0)
        {
            indent = indented ? indent : 0;
            var properties = Properties.Select(p => p.AsJson(indented, indent + 2));

            var jsonProperties = string.Join(indented ? "," + Environment.NewLine : ",", properties);

            return string.Format(indented ? "{{\r\n{0}\r\n{1}}}" : "{{{0}}}{1}", jsonProperties, new string(' ', indent));
        }
    }
}