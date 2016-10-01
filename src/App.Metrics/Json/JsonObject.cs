using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Json
{
    public class JsonObject
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