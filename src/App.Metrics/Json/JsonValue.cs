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
        private readonly string _value;

        public StringJsonValue(string value)
        {
            _value = value;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            return "\"" + Escape(_value) + "\"";
        }
    }

    public class LongJsonValue : JsonValue
    {
        private readonly long _value;

        public LongJsonValue(long value)
        {
            _value = value;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            return _value.ToString("D", CultureInfo.InvariantCulture);
        }
    }

    public class DoubleJsonValue : JsonValue
    {
        private readonly double _value;

        public DoubleJsonValue(double value)
        {
            _value = value;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            if (double.IsNaN(_value) || double.IsInfinity(_value))
            {
                return "null";
            }

            return _value.ToString("F", CultureInfo.InvariantCulture);
        }
    }

    public class BoolJsonValue : JsonValue
    {
        private readonly bool _value;

        public BoolJsonValue(bool value)
        {
            _value = value;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            return _value ? "true" : "false";
        }
    }

    public class ObjectJsonValue : JsonValue
    {
        private readonly JsonObject _value;

        public ObjectJsonValue(IEnumerable<JsonProperty> properties)
            : this(new JsonObject(properties))
        {
        }

        public ObjectJsonValue(JsonObject value)
        {
            _value = value;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            return _value.AsJson(indented, indent);
        }
    }

    public class CollectionJsonValue : JsonValue
    {
        private readonly IEnumerable<JsonObject> _values;

        public CollectionJsonValue(IEnumerable<JsonObject> values)
        {
            _values = values;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            return "[" + string.Join(",", _values.Select(e => e.AsJson(indented, indent))) + "]";
        }
    }

    public class StringArrayJsonValue : JsonValue
    {
        private readonly IEnumerable<string> _values;

        public StringArrayJsonValue(IEnumerable<string> values)
        {
            _values = values;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            return "[" + string.Join(",", _values.Select(v => new StringJsonValue(v).AsJson(indented, indent))) + "]";
        }
    }

    public class JsonValueArray : JsonValue
    {
        private readonly IEnumerable<JsonValue> _values;

        public JsonValueArray(IEnumerable<JsonValue> values)
        {
            _values = values;
        }

        public override string AsJson(bool indented = true, int indent = 0)
        {
            return "[" + string.Join(",", _values.Select(v => v.AsJson(indented, indent))) + "]";
        }
    }
}