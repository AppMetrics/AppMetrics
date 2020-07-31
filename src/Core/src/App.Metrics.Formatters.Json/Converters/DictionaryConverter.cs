using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace App.Metrics.Formatters.Json.Converters
{
    /// <summary>
    /// Enables reading/writing a dictionary as JSON.
    /// Implemented to support reading/writing non-standard keys.
    /// </summary>
    public class DictionaryConverter : JsonConverterFactory
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
            {
                return false;
            }

            var definition = typeToConvert.GetGenericTypeDefinition();

            return (definition == typeof(IDictionary<,>)
                    || definition == typeof(Dictionary<,>))
                   && typeToConvert.GetGenericArguments()[0] != typeof(string);
        }

        /// <inheritdoc />
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {

            if (!CanConvert(typeToConvert))
            {
                throw new InvalidOperationException($"{typeToConvert} is not a valid type for converter {typeof(DictionaryConverter<,>)}");
            }

            var converterType = typeof(DictionaryConverter<,>).MakeGenericType(typeToConvert.GetGenericArguments());
            return (JsonConverter)Activator.CreateInstance(converterType);
        }
    }

    /// <summary>
    /// Enables reading/writing a dictionary as JSON.
    /// Implemented to support reading/writing non-standard keys.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    public class DictionaryConverter<TKey, TValue> : JsonConverter<Dictionary<TKey, TValue>>
    {
        /// <inheritdoc />
        public override Dictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"Unexpected JsonToken '{reader.TokenType}' in converter {GetType()}");
            }

            reader.Read();

            var instance = new Dictionary<TKey, TValue>();
            while (reader.TokenType != JsonTokenType.EndObject)
            {
                var (key, value) = ReadEntry(ref reader, options);
                instance.Add(key, value);
            }

            return instance;
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Dictionary<TKey, TValue> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (var entry in value)
            {
                writer.WritePropertyName(JsonSerializer.Serialize(entry.Key, options));
                if (entry.Value is double)
                {
                    var d = Convert.ToDouble(entry.Value);
                    if (double.IsInfinity(d) || double.IsNaN(d) || double.IsNegativeInfinity(d) || double.IsPositiveInfinity(d))
                    {
                        JsonSerializer.Serialize(writer, entry.Value.ToString(), options);
                        continue;
                    }
                }
                JsonSerializer.Serialize(writer, entry.Value, options);
            }

            writer.WriteEndObject();
        }

        private (TKey Key, TValue Value) ReadEntry(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException($"Unexpected JsonToken '{reader.TokenType}' in converter {GetType()}");
            }

            var key = JsonSerializer.Deserialize<TKey>(reader.ValueSpan, options);

            reader.Read();

            var value = JsonSerializer.Deserialize<TValue>(ref reader, options);

            reader.Read();

            return (key, value);
        }
    }
}
