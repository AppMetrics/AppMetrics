using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace App.Metrics.Json
{
    public abstract class JsonValue
    {
        public static string Escape(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            return value.Replace(@"\", @"\\").Replace("\"", "\\\"").Replace("\r", @"\r").Replace("\n", @"\n");
        }

        public abstract string AsJson(bool indented = true, int indent = 0);
    }

    public class StringJsonValue : JsonValue
    {
        private readonly string value;

        public StringJsonValue(string value)
        {
            this.value = value;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            return "\"" + Escape(this.value) + "\"";
        }
    }

    public class LongJsonValue : JsonValue
    {
        private readonly long value;

        public LongJsonValue(long value)
        {
            this.value = value;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            return this.value.ToString("D", CultureInfo.InvariantCulture);
        }
    }

    public class DoubleJsonValue : JsonValue
    {
        private readonly double value;

        public DoubleJsonValue(double value)
        {
            this.value = value;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            if (double.IsNaN(this.value) || double.IsInfinity(this.value))
            {
                return "null";
            }

            return this.value.ToString("F", CultureInfo.InvariantCulture);
        }
    }

    public class BoolJsonValue : JsonValue
    {
        private readonly bool value;

        public BoolJsonValue(bool value)
        {
            this.value = value;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            return this.value ? "true" : "false";
        }
    }

    public class ObjectJsonValue : JsonValue
    {
        private readonly JsonObject value;

        public ObjectJsonValue(IEnumerable<JsonProperty> properties)
            : this(new JsonObject(properties))
        {
        }

        public ObjectJsonValue(JsonObject value)
        {
            this.value = value;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            return this.value.AsJson(indented, indent);
        }
    }

    public class CollectionJsonValue : JsonValue
    {
        private readonly IEnumerable<JsonObject> values;

        public CollectionJsonValue(IEnumerable<JsonObject> values)
        {
            this.values = values;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            return "[" + string.Join(",", this.values.Select(e => e.AsJson(indented, indent))) + "]";
        }
    }

    public class StringArrayJsonValue : JsonValue
    {
        private readonly IEnumerable<string> values;

        public StringArrayJsonValue(IEnumerable<string> values)
        {
            this.values = values;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            return "[" + string.Join(",", this.values.Select(v => new StringJsonValue(v).AsJson(indented, indent))) + "]";
        }
    }

    public class JsonValueArray : JsonValue
    {
        private readonly IEnumerable<JsonValue> values;

        public JsonValueArray(IEnumerable<JsonValue> values)
        {
            this.values = values;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            return "[" + string.Join(",", this.values.Select(v => v.AsJson(indented, indent))) + "]";
        }
    }
}